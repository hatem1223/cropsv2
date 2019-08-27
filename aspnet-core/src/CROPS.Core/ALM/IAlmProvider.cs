using System;
using System.Collections.Generic;
using CROPS.Projects;

namespace CROPS.ALM
{
    public interface IAlmProvider
    {
        void PrepairProjectForCreation(Project project);

        List<Assignee> GetAssigneesNames(int projectId);

        List<Iteration> GetIterationsList(int projectId);

        List<Bug> GetBugs(DateTime fromdate, DateTime? toDate);

        List<Epic> GetEpics(DateTime fromDate, DateTime? toDate);

        List<Task> GetTasks(DateTime fromdate, DateTime? toDate);

        List<Feature> GetFeatures(DateTime fromdate, DateTime? toDate);

        List<UserStory> GetUserStories(DateTime fromdate, DateTime? toDate);
    }
}
