using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using CROPS.ALM;
using CROPS.Configuration;
using CROPS.Projects.Contracts;
using CROPS.Projects.DTOs;
using CROPS.Reports;
using CROPS.Web;
using Microsoft.Extensions.Configuration;

namespace CROPS.Projects
{
    public class ProjectAppService : FilteredCrudAppService<Project, ProjectDto>, IProjectAppService
    {
        private readonly List<string> reservedWords;
        private readonly string invalidSpecialCharacters;
        private readonly List<string> endsWithRestrictions;
        private readonly List<string> startsWithRestrictions;

        private readonly IRepository<Bug> bugRepository;
        private readonly IRepository<Epic> epicRepository;
        private readonly IRepository<ALM.Task> taskRepository;
        private readonly IRepository<Project> projectRepository;
        private readonly IRepository<Feature> featureRepository;
        private readonly IRepository<Assignee> assigneeRepository;
        private readonly IRepository<UserStory> userStoryRepository;
        private readonly IRepository<Iteration> iterationRepository;
        private readonly IRepository<ProjectDetail> projectDetailsRepository;
        private readonly IIocResolver iocResolver;

        private IAlmProvider almProvider;
        private IReportsProvider powerBiProvider;

        public ProjectAppService(
              IRepository<Bug> bugRepository,
              IRepository<Epic> epicRepository,
              IReportsProvider powerBiProvider,
              IRepository<ALM.Task> taskRepository,
              IRepository<Project> projectRepository,
              IRepository<Feature> featureRepository,
              IRepository<Assignee> assigneeRepository,
              IRepository<UserStory> userStoryRepository,
              IRepository<Iteration> iterationRepository,
              IRepository<ProjectDetail> projectDetailsRepository,
              IIocResolver iocResolver,
              IConfiguration configuration)
            : base(projectRepository)
        {
            this.bugRepository = bugRepository;
            this.epicRepository = epicRepository;
            this.powerBiProvider = powerBiProvider;
            this.taskRepository = taskRepository;
            this.featureRepository = featureRepository;
            this.projectRepository = projectRepository;
            this.assigneeRepository = assigneeRepository;
            this.userStoryRepository = userStoryRepository;
            this.iterationRepository = iterationRepository;
            this.projectDetailsRepository = projectDetailsRepository;
            reservedWords = configuration["Configurations:reservedWords"].Split(',').ToList();
            invalidSpecialCharacters = configuration["Configurations:invalidSpecialCharacters"];
            endsWithRestrictions = configuration["Configurations:endsWithRestrictions"].Split(',').ToList();
            startsWithRestrictions = configuration["Configurations:startsWithRestrictions"].Split(',').ToList();
            this.iocResolver = iocResolver;
        }

        public override async Task<ProjectDto> Create(ProjectDto input)
        {
            CheckCreatePermission();
            ValidateProjectName(input.ProjectName);

            Project projectEntity = MapToEntity(input);
            bool existsInDB = projectRepository.GetAll().Any(a => a.ProjectName == projectEntity.ProjectName && a.ProjectAreaPath == projectEntity.ProjectAreaPath);
            if (existsInDB)
            {
                throw new UserFriendlyException("Project Was Registered Before!");
            }

            #region CheckWorkspaceExist & CreateWorkspace
            if (!string.IsNullOrEmpty(input.WorkspaceName))
            {
                var workspaceExist = await powerBiProvider.CheckWorkspaceExist(input.WorkspaceName).ConfigureAwait(false);
                if (workspaceExist)
                {
                    throw new UserFriendlyException("Workspace name is exist before!");
                }

                Microsoft.PowerBI.Api.V2.Models.Group workspace = powerBiProvider.CreateWorkspace(input.WorkspaceName).Result;
                projectEntity.WorkspaceId = workspace.Id;
            }
            #endregion

            #region Insert new project
            almProvider = iocResolver.Resolve<IAlmProvider>(input.IsOnPremProject ? "TFS" : "TFSOnline");
            almProvider.PrepairProjectForCreation(projectEntity);
            projectEntity = await projectRepository.InsertAsync(projectEntity).ConfigureAwait(false);
            CurrentUnitOfWork.SaveChanges();
            #endregion

            #region Insert All Project Iterations In To DB
            List<Iteration> iterationsList = almProvider.GetIterationsList(projectEntity.ProjectId);
            foreach (Iteration iteration in iterationsList)
            {
                Iteration existsIteration = iterationRepository.FirstOrDefault(i => i.IterationSourceId == iteration.IterationSourceId && i.ProjectId == projectEntity.ProjectId);
                if (existsIteration == null)
                {
                    iterationRepository.Insert(iteration);
                }
                else
                {
                    existsIteration.Depth = iteration.Depth;
                    existsIteration.Scope = iteration.Scope;
                    existsIteration.EndDate = iteration.EndDate;
                    existsIteration.StartDate = iteration.StartDate;
                    existsIteration.LastUpdatedDateTime = DateTime.UtcNow;
                    existsIteration.IterationName = iteration.IterationName;
                    iterationRepository.Update(existsIteration);
                }
            }
            #endregion

            #region Handle project work items
            List<Epic> epics = almProvider.GetEpics(input.FromDate, input.ToDate);
            await Sync(projectEntity.ProjectId, epics, epicRepository).ConfigureAwait(false);

            List<Feature> features = almProvider.GetFeatures(input.FromDate, input.ToDate);
            await Sync(projectEntity.ProjectId, features, featureRepository).ConfigureAwait(false);

            List<Bug> onlineBugsIdsList = almProvider.GetBugs(input.FromDate, input.ToDate);
            await Sync(projectEntity.ProjectId, onlineBugsIdsList, bugRepository).ConfigureAwait(false);

            List<ALM.Task> onlineTasksIdsList = almProvider.GetTasks(input.FromDate, input.ToDate);
            await Sync(projectEntity.ProjectId, onlineTasksIdsList, taskRepository).ConfigureAwait(false);

            List<UserStory> onlineUserStories = almProvider.GetUserStories(input.FromDate, input.ToDate);
            await Sync(projectEntity.ProjectId, onlineUserStories, userStoryRepository).ConfigureAwait(false);

            List<Assignee> onlineTeamMembersList = almProvider.GetAssigneesNames(projectEntity.ProjectId);
            await CompareAssigneesListAsync(onlineTeamMembersList, projectEntity.ProjectId).ConfigureAwait(false);
            #endregion

            #region Insert Project Details
            ProjectDetail projectDetails = new ProjectDetail
            {
                CreationDate = DateTime.UtcNow,
                PullDataFromDate = input.FromDate,
                ProjectId = projectEntity.ProjectId,
                PullDataToDate = input.ToDate ?? default(DateTime)
            };
            projectDetailsRepository.Insert(projectDetails);
            #endregion

            return MapToEntityDto(projectEntity);
        }

        private async System.Threading.Tasks.Task Sync<TWorkItem>(int projectId, IList<TWorkItem> items, IRepository<TWorkItem> repository)
            where TWorkItem : class, IWorkItem, IEntity
        {
            foreach (TWorkItem item in items)
            {
                var dbEpic = await repository.FirstOrDefaultAsync(e => e.ProjectId == item.ProjectId && e.SourceId == item.SourceId).ConfigureAwait(false);
                item.ProjectId = projectId;
                if (dbEpic == null)
                {
                    repository.Insert(item);
                }
                else
                {
                    dbEpic.CopyFrom(item);
                    repository.Update(dbEpic);
                }
            }

            var ids = items.Select(e => e.SourceId);
            var epicsToDelete = await repository.GetAllListAsync(e => e.ProjectId == projectId && !ids.Contains(e.SourceId)).ConfigureAwait(false);
            foreach (var epicToDelete in epicsToDelete)
            {
                epicToDelete.IsActive = 0;
            }
        }

        private async System.Threading.Tasks.Task CompareAssigneesListAsync(List<Assignee> onlineTeamMembersList, int registeredProjectId)
        {
            foreach (Assignee assignee in onlineTeamMembersList)
            {
                Assignee dbAssignee = await assigneeRepository.FirstOrDefaultAsync(a => a.ProjectId == registeredProjectId && a.Name == assignee.Name).ConfigureAwait(false);
                if (dbAssignee == null)
                {
                    assigneeRepository.Insert(assignee);
                }
                else
                {
                    dbAssignee.IsActive = assignee.IsActive;
                    assigneeRepository.Update(dbAssignee);
                }
            }

            var names = onlineTeamMembersList.Select(e => e.Name);
            var assigneesToDelete = await assigneeRepository.GetAllListAsync(e => e.ProjectId == registeredProjectId && !names.Contains(e.Name)).ConfigureAwait(false);
            foreach (var assigneeToDelete in assigneesToDelete)
            {
                assigneeToDelete.IsActive = 0;
            }
        }

        /// <summary>
        /// Validate the project name againest the naming restrictions and conventions
        /// ref: https://docs.microsoft.com/en-us/azure/devops/organizations/settings/naming-restrictions?view=azure-devops#project-and-work-item-tracking
        /// </summary>
        /// <param name="projectName">Project Name</param>
        private void ValidateProjectName(string projectName)
        {
            if (reservedWords.Contains(projectName))
            {
                throw new UserFriendlyException($"'{projectName}' is reserved. It cannot be used as project name.");
            }

            foreach (string chararcter in startsWithRestrictions)
            {
                if (projectName.StartsWith(chararcter, StringComparison.InvariantCulture))
                {
                    throw new UserFriendlyException($"Project name cannot start with '{chararcter}'");
                }
            }

            foreach (string chararcter in endsWithRestrictions)
            {
                if (projectName.EndsWith(chararcter, StringComparison.InvariantCulture))
                {
                    throw new UserFriendlyException($"Project name cannot end with '{chararcter}'");
                }
            }

            List<char> validationError = projectName.Intersect(invalidSpecialCharacters.ToCharArray()).ToList();
            if (validationError.Any())
            {
                throw new UserFriendlyException($"Project name cannot contain '{string.Join(", ", validationError)}'");
            }
        }
    }
}
