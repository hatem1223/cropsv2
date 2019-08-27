using Abp.Application.Services;
using Abp.Domain.Repositories;
using CROPS.Reports;
using CROPS.Reports.Contracts;
using CROPS.Reports.DTOs;
using CROPS.WorkSpace.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CROPS.WorkSpace
{
    public class WorkspaceAppService : ApplicationService, IWorkspaceAppService
    {
        private readonly IRepository<Workspace, string> repository;

        public WorkspaceAppService(IRepository<Workspace, string> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Get All Workspaces including workspaces and Dashboards and reports
        /// </summary>
        /// <returns> List<WorkspaceDto> </returns>
        public List<WorkspaceDto> GetAll()
        {
            var workspacesfromDB = repository.GetAllIncluding(x => x.Dashboards, y => y.Reports).Where(x => x.IsActive).ToList();
            var workspaces = ObjectMapper.Map<List<WorkspaceDto>>(workspacesfromDB);

            for (int i = 0; i < workspaces.Count; i++)
            {
                workspaces[i].Dashboards = ObjectMapper.Map<List<DashboardDto>>(workspacesfromDB[i].Dashboards.Where(d => d.IsActive));
                workspaces[i].Reports = ObjectMapper.Map<List<ReportDto>>(workspacesfromDB[i].Reports.Where(r => r.IsActive));
            }

            return workspaces;
        }
    }
}
