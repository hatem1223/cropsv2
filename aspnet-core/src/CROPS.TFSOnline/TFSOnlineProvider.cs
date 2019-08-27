using Abp.Domain.Repositories;
using Abp.UI;
using CROPS.ALM;
using CROPS.Projects;
using CROPS.Utilities;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CROPS.TFSOnline
{
    public class TFSOnlineProvider : IAlmProvider
    {
        TeamHttpClient _teamClient;
        QueryHierarchyItem _queriesFolder;
        WorkItemTrackingHttpClient _workItemTrackingClient;

        readonly IEncryption encryption;
        readonly IRepository<Project> projectRepository;

        const string BugsQueryName = "Bugs";
        const string EpicsQueryName = "Epics";
        const string TasksQueryName = "Tasks";
        const string QueriesFolderName = "CROPS";
        const string QuieriesFolder = "My Queries";
        const string FeaturesQueryName = "Features";
        const string UserStoriesQueryName = "UserStories";
        const string DecryptionKey = "E546C8DF278CD5931069B522E695D4F2";
        const string ParentRelationType = "System.LinkTypes.Hierarchy-Reverse";

        public string OnlineTfsProjectId { get; set; }
        public string OnlineTfsTeamProjectName { get; set; }


        public TFSOnlineProvider(
              IEncryption encryption
            , IRepository<Project> projectRepository)
        {
            this.encryption = encryption;
            this.projectRepository = projectRepository;
        }

        public void PrepairProjectForCreation(Project project)
        {
            ProjectHttpClient projClient;
            TeamProjectReference onlineProject;
            Uri tfsUri = new Uri(project.ProjectAreaPath);
            VssBasicCredential credentials = new VssBasicCredential(project.UserName, project.Password);
            VssConnection connection = new VssConnection(tfsUri, credentials);
            _teamClient = connection.GetClientAsync<TeamHttpClient>().Result;
            projClient = connection.GetClientAsync<ProjectHttpClient>().Result;
            _workItemTrackingClient = connection.GetClient<WorkItemTrackingHttpClient>();
            IPagedList<TeamProjectReference> projects = projClient.GetProjects().Result;
            onlineProject = projects.FirstOrDefault(pro => pro.Name.ToUpperInvariant().Equals(project.ProjectName.ToUpperInvariant(), StringComparison.InvariantCulture));
            if (onlineProject != null)
            {
                project.CreationDate = DateTime.Now;
                OnlineTfsTeamProjectName = project.ProjectName;
                OnlineTfsProjectId = onlineProject.Id.ToString();
                project.ProjectType = Projects.Enums.ProjectType.Online.ToString();
                project.Password = project.AuthType == 1 ? encryption.DecryptString(project.Password, DecryptionKey) : project.Password;
                ConnectToOnLineTfsAndCreateQuries(project.ProjectName);
            }
            else
            {
                throw new UserFriendlyException($"Project Path: '{project.ProjectAreaPath}' Was Not Found!");
            }
        }

        void ConnectToOnLineTfsAndCreateQuries(string projectName)
        {
            List<QueryHierarchyItem> queryHierarchyItems = _workItemTrackingClient.GetQueriesAsync(projectName, depth: 2).Result;
            QueryHierarchyItem sharedQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals(QuieriesFolder, StringComparison.InvariantCulture));
            if (sharedQueriesFolder != null)
            {
                _queriesFolder = sharedQueriesFolder.Children?.FirstOrDefault(qhi => qhi.Name.Equals(QueriesFolderName, StringComparison.InvariantCulture));
                if (_queriesFolder == null)
                {
                    QueryHierarchyItem queriesFolder = new QueryHierarchyItem
                    {
                        IsFolder = true,
                        HasChildren = true,
                        Name = QueriesFolderName
                    };
                    _queriesFolder = _workItemTrackingClient.CreateQueryAsync(queriesFolder, projectName, sharedQueriesFolder.Name).Result;
                }
            }
        }


        public List<Assignee> GetAssigneesNames(int projectId)
        {
            List<Assignee> onlineTeamMembersList = new List<Assignee>();
            List<WebApiTeam> teams = _teamClient.GetTeamsAsync(OnlineTfsProjectId).Result;
            foreach (WebApiTeam team in teams)
            {
                List<TeamMember> teamMembers = _teamClient.GetTeamMembersWithExtendedPropertiesAsync(OnlineTfsProjectId, team.Id.ToString()).Result;
                foreach (TeamMember member in teamMembers)
                {
                    onlineTeamMembersList.Add(new Assignee
                    {
                        IsActive = 1,
                        ProjectId = projectId,
                        Name = member.Identity.DisplayName
                    });
                }
            }
            return onlineTeamMembersList;
        }

        public List<Iteration> GetIterationsList(int projectId)
        {
            List<Iteration> iterationList = new List<Iteration>();
            List<WorkItemClassificationNode> projectIterations = _workItemTrackingClient.GetClassificationNodeAsync(OnlineTfsTeamProjectName, TreeStructureGroup.Iterations, depth: 5).Result.Children.ToList();
            foreach (WorkItemClassificationNode iteration in projectIterations)
            {
                iterationList.Add(new Iteration
                {
                    Depth = 1,
                    Scope = null,
                    ProjectId = projectId,
                    IterationName = iteration.Name,
                    IterationSourceId = iteration.Id,
                    LastUpdatedDateTime = DateTime.UtcNow,
                    EndDate = Convert.ToDateTime(iteration.Attributes?["finishDate"], CultureInfo.InvariantCulture),
                    StartDate = Convert.ToDateTime(iteration.Attributes?["startDate"], CultureInfo.InvariantCulture)
                });
            }
            return iterationList;
        }

        #region Create Queries

        public List<Epic> GetEpics(DateTime fromDate, DateTime? toDate)
        {
            List<Epic> result = new List<Epic>();
            QueryHierarchyItem epicsQuery = _queriesFolder.Children?.FirstOrDefault(qhi => qhi.Name.Equals(EpicsQueryName, StringComparison.InvariantCulture));
            if (epicsQuery == null)
            {
                epicsQuery = new QueryHierarchyItem
                {
                    Name = EpicsQueryName,
                    Wiql = string.Format(CultureInfo.InvariantCulture,
                                                 "SELECT[System.Id], " +
                                                 "[System.IterationId]," +
                                                 "[System.CreatedDate]," +
                                                 "[System.Title]," +
                                                 "[System.State]" +
                                                 "FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Epic' AND [System.State] <> 'Removed'  AND [System.CreatedDate] >= '{0:MM/dd/yyyy}'", fromDate),
                    IsFolder = false
                };
                if (toDate?.Year != 1)
                {
                    epicsQuery.Wiql += string.Format(CultureInfo.InvariantCulture, Environment.NewLine + "AND [System.CreatedDate] <= '{1:MM/dd/yyyy}'", toDate);
                }
            }
            epicsQuery = _workItemTrackingClient.CreateQueryAsync(epicsQuery, OnlineTfsTeamProjectName, _queriesFolder.Id.ToString()).Result;
            WorkItemQueryResult epicsQueryResult = _workItemTrackingClient.QueryByIdAsync(epicsQuery.Id).Result;
            if (epicsQueryResult.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = epicsQueryResult.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        List<string> columnptions = new List<string>
                        {
                            "System.Id", "System.IterationId", "System.CreatedDate", "System.Title", "System.State"
                        };
                        List<WorkItem> workItems = _workItemTrackingClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id), columnptions).Result;
                        foreach (WorkItem workItem in workItems)
                        {
                            Epic epic = new Epic();
                            FillEpicsData(epic, workItem.Fields);
                            result.Add(epic);
                        }
                    }
                    skip += batchSize;
                } while (workItemRefs.Count() == batchSize);
            }
            return result;
        }

        public List<Feature> GetFeatures(DateTime fromdate, DateTime? toDate)
        {
            List<Feature> features = new List<Feature>();
            QueryHierarchyItem featuresQuery = _queriesFolder.Children?.FirstOrDefault(qhi => qhi.Name.Equals(FeaturesQueryName, StringComparison.InvariantCulture));
            if (featuresQuery == null)
            {
                featuresQuery = new QueryHierarchyItem()
                {
                    Name = FeaturesQueryName,
                    Wiql = string.Format(CultureInfo.InvariantCulture,
                                                     "SELECT [System.Id]," +
                                                     "[System.IterationId]," +
                                                     "[System.CreatedDate]," +
                                                     "[System.Title]," +
                                                     "[System.State]" +
                                                     "FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Feature' AND [System.State] <> 'Removed' AND [System.CreatedDate] >= '{0:MM/dd/yyyy}'", fromdate),
                    IsFolder = false
                };
                if (toDate?.Year != 1)
                {
                    featuresQuery.Wiql += string.Format(CultureInfo.InvariantCulture, Environment.NewLine + "AND [System.CreatedDate] <= '{1:MM/dd/yyyy}'", toDate);
                }
            }
            featuresQuery = _workItemTrackingClient.CreateQueryAsync(featuresQuery, OnlineTfsTeamProjectName, _queriesFolder.Id.ToString()).Result;
            WorkItemQueryResult featuresQueryResult = _workItemTrackingClient.QueryByIdAsync(featuresQuery.Id).Result;
            if (featuresQueryResult.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = featuresQueryResult.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        List<string> columnptions = new List<string>
                        {
                            "System.Id", "System.IterationId", "System.CreatedDate", "System.Title", "System.State"
                        };
                        List<WorkItem> workItems = _workItemTrackingClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id), columnptions).Result;
                        foreach (WorkItem workItem in workItems)
                        {
                            Feature feature = new Feature();
                            FillFeaturesData(feature, workItem.Fields);
                            features.Add(feature);
                        }
                    }
                    skip += batchSize;
                } while (workItemRefs.Count() == batchSize);
            }
            return features;
        }

        public List<UserStory> GetUserStories(DateTime fromdate, DateTime? toDate)
        {
            List<UserStory> onlineUserStories = new List<UserStory>();
            QueryHierarchyItem userStoriesQuery = _queriesFolder.Children?.FirstOrDefault(qhi => qhi.Name.Equals(UserStoriesQueryName, StringComparison.InvariantCulture));
            if (userStoriesQuery == null)
            {
                userStoriesQuery = new QueryHierarchyItem
                {
                    Name = UserStoriesQueryName,
                    Wiql = string.Format(CultureInfo.InvariantCulture,
                                             "SELECT [System.Id]," +
                                             "[System.IterationId]," +
                                             "[System.CreatedDate]," +
                                             "[System.Title]," +
                                             "[System.State]," +
                                             "[System.AssignedTo]," +
                                             "[microsoft.vsts.scheduling.StoryPoints]" +
                                             "FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'User Story' AND [System.State] <> 'Removed' AND[System.CreatedDate] >= '{0:MM / dd / yyyy}'", fromdate),
                    IsFolder = false
                };
                if (toDate?.Year != 1)
                {
                    userStoriesQuery.Wiql += string.Format(CultureInfo.InvariantCulture, Environment.NewLine + "AND [System.CreatedDate] <= '{1:MM/dd/yyyy}'", toDate);
                }
            }
            userStoriesQuery = _workItemTrackingClient.CreateQueryAsync(userStoriesQuery, OnlineTfsTeamProjectName, _queriesFolder.Id.ToString()).Result;
            WorkItemQueryResult userStoriesQueryResult = _workItemTrackingClient.QueryByIdAsync(userStoriesQuery.Id).Result;
            if (userStoriesQueryResult.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = userStoriesQueryResult.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        var workItems = _workItemTrackingClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id), expand: WorkItemExpand.All).Result;
                        foreach (var workItem in workItems)
                        {
                            int parentId = 0;
                            if (workItem.Relations != null)
                            {
                                foreach (var relation in workItem.Relations)
                                {
                                    if (!relation.Rel.ToUpperInvariant().Equals(ParentRelationType.ToUpperInvariant(), StringComparison.InvariantCulture)) continue;
                                    string path = relation.Url;
                                    int pos = path.LastIndexOf("/", StringComparison.Ordinal) + 1;
                                    int.TryParse(path.Substring(pos, path.Length - pos), out parentId);
                                }
                            }
                            UserStory userStory = new UserStory();
                            FillUserStorysData(userStory, workItem.Fields, parentId);
                            onlineUserStories.Add(userStory);
                        }
                    }
                    skip += batchSize;
                } while (workItemRefs.Count() == batchSize);
            }
            return onlineUserStories;
        }

        public List<ALM.Task> GetTasks(DateTime fromdate, DateTime? toDate)
        {
            List<ALM.Task> onlineTasks = new List<ALM.Task>();
            QueryHierarchyItem tasksQuery = _queriesFolder.Children?.FirstOrDefault(qhi => qhi.Name.Equals(TasksQueryName, StringComparison.InvariantCulture));
            if (tasksQuery == null)
            {
                tasksQuery = new QueryHierarchyItem
                {
                    Name = TasksQueryName,
                    Wiql = string.Format(CultureInfo.InvariantCulture,
                                         "SELECT [System.Id]," +
                                         "[System.IterationId]," +
                                         "[System.CreatedDate]," +
                                         "[System.Title]," +
                                         "[System.State]," +
                                         "[System.AssignedTo]," +
                                         "[Microsoft.VSTS.Scheduling.CompletedWork]," +
                                         "[Microsoft.VSTS.Scheduling.OriginalEstimate]," +
                                         "[Microsoft.VSTS.Scheduling.RemainingWork]," +
                                         "[Microsoft.VSTS.Common.Activity]" +
                                         "FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Task' AND [System.State] <> 'Removed' AND[System.CreatedDate] >= '{0:MM / dd / yyyy}'", fromdate),
                    IsFolder = false
                };
                if (toDate?.Year != 1)
                {
                    tasksQuery.Wiql += string.Format(CultureInfo.InvariantCulture, Environment.NewLine + "AND [System.CreatedDate] <= '{1:MM/dd/yyyy}'", toDate);
                }
                tasksQuery = _workItemTrackingClient.CreateQueryAsync(tasksQuery, OnlineTfsTeamProjectName, _queriesFolder.Id.ToString()).Result;
            }
            WorkItemQueryResult tasksQueryResult = _workItemTrackingClient.QueryByIdAsync(tasksQuery.Id).Result;
            if (tasksQueryResult.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = tasksQueryResult.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        var workItems = _workItemTrackingClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id), expand: WorkItemExpand.All).Result;
                        foreach (var workItem in workItems)
                        {
                            int parentId = 0;
                            if (workItem.Relations != null)
                            {
                                foreach (var relation in workItem.Relations)
                                {
                                    if (!relation.Rel.ToUpperInvariant().Equals(ParentRelationType.ToUpperInvariant(), StringComparison.InvariantCulture)) continue;
                                    string path = relation.Url;
                                    int pos = path.LastIndexOf("/", StringComparison.Ordinal) + 1;
                                    int.TryParse(path.Substring(pos, path.Length - pos), out parentId);
                                }
                            }
                            ALM.Task task = new ALM.Task();
                            FillTasksData(task, workItem.Fields, parentId);
                            onlineTasks.Add(task);
                        }
                    }
                    skip += batchSize;
                } while (workItemRefs.Count() == batchSize);
            }
            return onlineTasks;
        }

        public List<Bug> GetBugs(DateTime fromdate, DateTime? toDate)
        {
            List<Bug> onlineBugs = new List<Bug>();
            QueryHierarchyItem bugsQuery = _queriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(BugsQueryName, StringComparison.InvariantCulture));
            if (bugsQuery == null)
            {
                bugsQuery = new QueryHierarchyItem()
                {
                    Name = BugsQueryName,
                    Wiql = string.Format(CultureInfo.InvariantCulture,
                                             "SELECT [System.Id]," +
                                             "[System.IterationId]," +
                                             "[System.CreatedDate]," +
                                             "[System.Title]," +
                                             "[System.State]," +
                                             "[System.AssignedTo]," +
                                             "[Microsoft.VSTS.Scheduling.CompletedWork]," +
                                             "[Microsoft.VSTS.Scheduling.OriginalEstimate]," +
                                             "[Microsoft.VSTS.Scheduling.RemainingWork]," +
                                             "[Microsoft.VSTS.Common.Activity]" +
                                             "FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Bug' AND [System.State] <> 'Removed' AND[System.CreatedDate] >= '{0:MM / dd / yyyy}'", fromdate),
                    IsFolder = false
                };
                if (toDate?.Year != 1)
                {
                    bugsQuery.Wiql += string.Format(CultureInfo.InvariantCulture, Environment.NewLine + "AND [System.CreatedDate] <= '{1:MM/dd/yyyy}'", toDate);
                }
            }
            bugsQuery = _workItemTrackingClient.CreateQueryAsync(bugsQuery, OnlineTfsTeamProjectName, _queriesFolder.Id.ToString()).Result;
            WorkItemQueryResult bugsQueryResult = _workItemTrackingClient.QueryByIdAsync(bugsQuery.Id).Result;
            if (bugsQueryResult.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = bugsQueryResult.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        List<string> columnptions = new List<string>
                        {
                            "Microsoft.VSTS.Scheduling.RemainingWork", "Microsoft.VSTS.Common.Activity",
                            "Microsoft.VSTS.Scheduling.CompletedWork", "Microsoft.VSTS.Scheduling.OriginalEstimate",
                            "System.Id", "System.IterationId", "System.CreatedDate", "System.Title", "System.State", "System.AssignedTo"
                        };
                        var workItems = _workItemTrackingClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id), columnptions).Result;
                        foreach (var workItem in workItems)
                        {
                            Bug bug = new Bug();
                            FillBugsData(bug, workItem.Fields);
                            onlineBugs.Add(bug);
                        }
                    }
                    skip += batchSize;
                } while (workItemRefs.Count() == batchSize);
            }
            return onlineBugs;
        }

        private void FillEpicsData(Epic epic, IDictionary<string, object> fields)
        {
            foreach (var field in fields)
            {
                var key = field.Key;
                var value = field.Value;

                if (key.ToUpperInvariant().Contains("System.Id".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    epic.SourceId = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.IterationId".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    epic.IterationSk = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.CreatedDate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    epic.CreationDate = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.Title".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    epic.EpicName = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("System.State".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    epic.State = value.ToString();
                }
            }
            epic.IsActive = 1;
        }

        private void FillFeaturesData(Feature feature, IDictionary<string, object> fields)
        {
            foreach (var field in fields)
            {
                var key = field.Key;
                var value = field.Value;

                if (key.ToUpperInvariant().Contains("System.Id".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    feature.SourceId = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.IterationId".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    feature.IterationSk = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.CreatedDate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    feature.CreationDate = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.Title".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    feature.FeatureName = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("System.State".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    feature.State = value.ToString();
                }
            }
            feature.IsActive = 1;
        }

        private void FillUserStorysData(UserStory userStory, IDictionary<string, object> fields, int? parentId)
        {
            foreach (var field in fields)
            {
                var key = field.Key;
                var value = field.Value;

                if (key.ToUpperInvariant().Contains("System.Id".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.SourceId = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.IterationId".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.IterationSk = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.CreatedDate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.CreationDate = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.AssignedTo".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.AssignedTo = ((IdentityRef)value).DisplayName;
                }

                if (key.ToUpperInvariant().Contains("System.Title".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.UserStoryName = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("System.State".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.State = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("microsoft.vsts.scheduling.StoryPoints".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    userStory.StoryPoints = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }
            }
            userStory.IsActive = 1;
            userStory.FeatureSourceId = parentId != 0 ? parentId : null;
        }

        private void FillTasksData(ALM.Task task, IDictionary<string, object> fields, int? parentId)
        {
            foreach (var field in fields)
            {
                var key = field.Key;
                var value = field.Value;

                if (key.ToUpperInvariant().Contains("System.Id".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.SourceId = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.IterationId".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.IterationSk = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.CreatedDate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.CreationDate = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.AssignedTo".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.AssignedTo = ((IdentityRef)value).DisplayName;
                }

                if (key.ToUpperInvariant().Contains("System.Title".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.TaskName = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("System.State".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.State = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Scheduling.CompletedWork".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.CompletedWork = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Scheduling.OriginalEstimate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.OriginalEstimate = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Scheduling.RemainingWork".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.RemainingWork = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Common.Activity".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    task.Activity = value.ToString();
                }
            }
            task.IsActive = 1;
            task.UserStorySourceId = parentId != 0 ? parentId : null;
        }

        private void FillBugsData(Bug bug, IDictionary<string, object> fields)
        {
            foreach (var field in fields)
            {
                var key = field.Key;
                var value = field.Value;

                if (key.ToUpperInvariant().Contains("System.Id".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.SourceId = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.IterationId".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.IterationSk = Convert.ToInt32(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.CreatedDate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.CreationDate = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("System.AssignedTo".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.AssignedTo = ((IdentityRef)value).DisplayName;
                }

                if (key.ToUpperInvariant().Contains("System.Title".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.BugName = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("System.State".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.State = value.ToString();
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Scheduling.CompletedWork".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.CompletedWork = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Scheduling.OriginalEstimate".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.OriginalEstimate = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Scheduling.RemainingWork".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.RemainingWork = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                }

                if (key.ToUpperInvariant().Contains("Microsoft.VSTS.Common.Activity".ToUpperInvariant(), StringComparison.InvariantCulture))
                {
                    bug.Activity = value.ToString();
                }
            }
            bug.IsActive = 1;
            bug.UserStorySourceId = null;
        }
        #endregion
    }
}
