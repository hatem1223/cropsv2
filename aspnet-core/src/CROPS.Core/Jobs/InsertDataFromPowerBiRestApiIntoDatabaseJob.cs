using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using CROPS.Projects;
using CROPS.Reports;
using Microsoft.PowerBI.Api.V2.Models;

namespace CROPS.Jobs
{
    public class InsertDataFromPowerBiRestApiIntoDatabaseJob : DomainService, IInsertDataFromPowerBiRestApiIntoDatabaseJob
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IReportsProvider powerBIProvider;
        private IRepository<Workspace, string> workspaceRepository;
        private IRepository<Reports.Report, string> reportRepository;
        private IRepository<Project, int> projectRepository;
        private IRepository<Reports.Dashboard, string> dashboardRepository;

        public InsertDataFromPowerBiRestApiIntoDatabaseJob(IUnitOfWorkManager unitOfWorkManager, IReportsProvider powerBIProvider, IRepository<Workspace, string> workspaceRepository, IRepository<Reports.Report, string> reportRepository, IRepository<Project, int> projectRepository, IRepository<Reports.Dashboard, string> dashboardRepository)
        {
            Id = "InsertDataFromPowerBiRestApiIntoDatabaseJob";
            this.powerBIProvider = powerBIProvider;
            this.workspaceRepository = workspaceRepository;
            this.reportRepository = reportRepository;
            this.projectRepository = projectRepository;
            this.dashboardRepository = dashboardRepository;
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public string Id { get; set; }

        public virtual async Task Execute()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var allWorkspacesFromAPI = powerBIProvider.GetAllWorkspaces();
                DeActiveDeletedWorkspaces(allWorkspacesFromAPI);
                foreach (var apiWorkspace in allWorkspacesFromAPI)
                {
                    DeActiveDeletedReports(apiWorkspace.Id);
                    DeActiveDeletedDashboards(apiWorkspace.Id);
                    var projectWorkspace = GetProjectOfWorkspace(apiWorkspace);
                    if (projectWorkspace != null)
                    {
                        List<Reports.Report> reports = GetReportsOfWorkspace(projectWorkspace);
                        await powerBIProvider.CheckMasterReportsWithProjectsWorkspaces(projectWorkspace, reports);
                    }

                    Workspace workspaceObject = await GetWorkSpaceDB(apiWorkspace);
                    if (workspaceObject != null)
                    {
                        if (workspaceObject.Name != apiWorkspace.Name)
                        {
                            workspaceObject.Name = apiWorkspace.Name;
                            workspaceRepository.Update(workspaceObject);
                        }

                        InsertReportsAndDashboards(apiWorkspace, workspaceObject, true);
                    }
                    else
                    {
                        workspaceObject = new Workspace
                        {
                            WorkspaceId = apiWorkspace.Id,
                            Name = apiWorkspace.Name,
                            IsActive = true
                        };

                        workspaceRepository.Insert(workspaceObject);

                        InsertReportsAndDashboards(apiWorkspace, workspaceObject, false);

                    }
                }
                unitOfWork.Complete();
            }
        }

        public virtual void InsertReportsAndDashboards(Group apiWorkspace, Workspace workspaceObject, bool isExistedWorkspace)
        {
            InsertReportsInToWorkspace(apiWorkspace, workspaceObject, isExistedWorkspace);

            InsertDashboardsInToWorkspace(apiWorkspace, workspaceObject, isExistedWorkspace);
        }

        public async Task<Workspace> GetWorkSpaceDB(Group apiWorkspace)
        {
            return await workspaceRepository.FirstOrDefaultAsync(ws => ws.WorkspaceId == apiWorkspace.Id);
        }

        public List<Reports.Report> GetReportsOfWorkspace(Project projectWorkspace)
        {
            return reportRepository.GetAll().Where(report => report.Workspace.WorkspaceId == projectWorkspace.WorkspaceId && report.IsActive).ToList();
        }

        public Project GetProjectOfWorkspace(Group apiWorkspace)
        {
            return projectRepository.GetAll().FirstOrDefault(x => x.WorkspaceId == apiWorkspace.Id);
        }

        public void DeActiveDeletedWorkspaces(List<Group> allWorkspacesFromAPI)
        {
            var allWorkspacesFromDB = workspaceRepository.GetAllList();

            foreach (var dbWorkspace in allWorkspacesFromDB)
            {
                if (allWorkspacesFromAPI.ToList().Exists(ws => ws.Id == dbWorkspace.WorkspaceId))
                {
                    continue;
                }
                else
                {
                    dbWorkspace.IsActive = false;
                }
            }
        }

        public void DeActiveDeletedReports(string apiWorkspaceId)
        {
            var reportsInApiWorkspace = powerBIProvider.GetAllReportsInWorkspace(apiWorkspaceId);
            var reportsInDBWorkspace = GetAllReportsInWorkspaceDB(apiWorkspaceId);
            foreach (var dbReport in reportsInDBWorkspace)
            {
                if (reportsInApiWorkspace.ToList().Exists(apiReport => apiReport.Id == dbReport.ReportId))
                {
                    continue;
                }
                else
                {
                    dbReport.IsActive = false;
                }
            }
        }

        public List<Reports.Report> GetAllReportsInWorkspaceDB(string apiWorkspaceId)
        {
            return reportRepository.GetAll().Where(report => report.Workspace.WorkspaceId == apiWorkspaceId).ToList();
        }

        public void DeActiveDeletedDashboards(string apiWorkspaceId)
        {
            var dashboardsInApiWorkspace = powerBIProvider.GetAllDashboardsInWorkspace(apiWorkspaceId);
            var dashboardsInDBWorkspace = dashboardRepository.GetAll().Where(dashboard => dashboard.Workspace.WorkspaceId == apiWorkspaceId);
            foreach (var dbDashboard in dashboardsInDBWorkspace)
            {
                if (dashboardsInApiWorkspace.ToList().Exists(apiDashboard => apiDashboard.Id == dbDashboard.DashboardId))
                {
                    continue;
                }
                else
                {
                    dbDashboard.IsActive = false;
                }
            }
        }

        public void InsertReportsInToWorkspace(Group workspace, Workspace workspaceObject, bool isExistWorkspace)
        {
            var workspaceReports = powerBIProvider.GetAllReportsInWorkspace(workspace.Id);

            foreach (var workspaceReport in workspaceReports)
            {
                var reportsObject = reportRepository.FirstOrDefault(i => i.ReportId == workspaceReport.Id);

                if (isExistWorkspace && reportsObject != null)
                {
                    if (reportsObject.Name != workspaceReport.Name)
                    {
                        reportsObject.Name = workspaceReport.Name;
                        reportRepository.Update(reportsObject);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    reportsObject = new Reports.Report
                    {
                        ReportId = workspaceReport.Id,
                        Name = workspaceReport.Name,
                        WorkspaceId = workspaceObject.WorkspaceId,
                        IsActive = true
                    };
                    reportRepository.Insert(reportsObject);
                }
            }
        }

        public void InsertDashboardsInToWorkspace(Group workspace, Workspace workspaceObject, bool isExistWorkspace)
        {
            var workspaceDashboards = powerBIProvider.GetAllDashboardsInWorkspace(workspace.Id);

            foreach (var workspaceDashboard in workspaceDashboards)
            {
                var dashboardObject = dashboardRepository.FirstOrDefault(i => i.DashboardId == workspaceDashboard.Id);

                if (isExistWorkspace && dashboardObject != null)
                {
                    if (dashboardObject.Name != workspaceDashboard.DisplayName)
                    {
                        dashboardObject.Name = workspaceDashboard.DisplayName;
                        dashboardRepository.Update(dashboardObject);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    dashboardObject = new Reports.Dashboard
                    {
                        DashboardId = workspaceDashboard.Id,
                        Name = workspaceDashboard.DisplayName,
                        WorkspaceId = workspaceObject.WorkspaceId,
                        IsActive = true
                    };

                    dashboardRepository.Insert(dashboardObject);
                }
            }
        }
    }
}
