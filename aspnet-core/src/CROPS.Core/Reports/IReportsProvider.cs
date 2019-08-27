using CROPS.Projects;
using Microsoft.PowerBI.Api.V2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CROPS.Reports
{
    public interface IReportsProvider
    {
        Task<string> GetReportUrlAsync(string groupId, string reportId);

        Task<Microsoft.PowerBI.Api.V2.Models.Dashboard> GetDashboardAsync(string groupId, string dashboardId);

        void GetReports();

        string GetAccessToken();

        List<Group> GetAllWorkspaces();

        List<Microsoft.PowerBI.Api.V2.Models.Report> GetAllReportsInWorkspace(string workspaceId);

        List<Microsoft.PowerBI.Api.V2.Models.Dashboard> GetAllDashboardsInWorkspace(string workspaceId);
        Task CheckMasterReportsWithProjectsWorkspaces(Project projectWorkspace, List<Report> reports);

        Task<Group> CreateWorkspace(string name);

        Task<bool> CheckWorkspaceExist(string name);
    }
}
