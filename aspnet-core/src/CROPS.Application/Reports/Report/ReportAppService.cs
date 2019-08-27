using System;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using CROPS.Projects;
using CROPS.Projects.Contracts;
using CROPS.Reports.Contracts;
using CROPS.Reports.DTOs;
using System.Linq;

namespace CROPS.Reports
{
    public class ReportAppService : ApplicationService, IReportAppService
    {
        private readonly IRepository<Report, string> reportrepository;
        private readonly IRepository<Project> projectrepository;
        private readonly IReportsProvider reportsProvider;

        public ReportAppService(
            IRepository<Report, string> repository,
            IReportsProvider reportsProvider,
            IRepository<Project> projectrepository)
        {
            this.reportrepository = repository;
            this.reportsProvider = reportsProvider;
            this.projectrepository = projectrepository;
        }

        public ReportDetailsDto Get(string id)
        {
            var report = reportrepository.Get(id);
            var projectWorkspace = projectrepository.GetAll().FirstOrDefault(x => x.WorkspaceId == report.WorkspaceId);
            var projectId = projectWorkspace.Id;
            var reportDetails = ObjectMapper.Map<ReportDetailsDto>(report);

            reportDetails.Url = reportsProvider.GetReportUrlAsync(report.Id, id) + "&filter=Project/ProjectId eq " + projectId;

            return reportDetails;
        }
    }
}
