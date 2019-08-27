using Abp.UI;
using CROPS.ALM;
using CROPS.EntityFrameworkCore.Repositories;
using CROPS.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPS.TFS
{
    public class TFSProvider : IAlmProvider
    {
        private readonly IProjectRepository repository;

        public TFSProvider(IProjectRepository repository)
        {
            this.repository = repository;
        }

        public List<Iteration> GetIterationsList(int projectId)
        {
            return new List<Iteration>();
        }

        public void PrepairProjectForCreation(Project project)
        {
            Task<SP_GetProjectDataByProjectAreaPath_Result> sp_GetProjectDataByProjectAreaPath_Result = repository.GetProjectDataByProjectAreaPath(project.ProjectAreaPath);           
            if (sp_GetProjectDataByProjectAreaPath_Result.Result == null)
            {
                throw new UserFriendlyException($"Project Path: '{project.ProjectAreaPath}' Was Not Found!");
            }
            project.IsActive = true;
            project.CreationDate = DateTime.Now;
            project.ProjectType = Projects.Enums.ProjectType.OnPrem.ToString();
            project.ParentSourceId = sp_GetProjectDataByProjectAreaPath_Result.Result.ParentNodeSK;
            project.ProjectSourceId = sp_GetProjectDataByProjectAreaPath_Result.Result.ProjectNodeSK;
            project.ReleaseSourceId = sp_GetProjectDataByProjectAreaPath_Result.Result.ReleaseSourceId;            
        }

        public List<Bug> GetBugs(DateTime fromdate, DateTime? toDate)
        {
            return new List<Bug>();
        }

        public List<Epic> GetEpics(DateTime fromDate, DateTime? toDate)
        {
            return new List<Epic>();
        }

        public List<Assignee> GetAssigneesNames(int projectId)
        {
            return new List<Assignee>();
        }

        public List<ALM.Task> GetTasks(DateTime fromdate, DateTime? toDate)
        {
            return new List<ALM.Task>();
        }

        public List<Feature> GetFeatures(DateTime fromdate, DateTime? toDate)
        {
            return new List<Feature>();
        }

        public List<UserStory> GetUserStories(DateTime fromdate, DateTime? toDate)
        {
            return new List<UserStory>();
        }
    }
}
