using Abp.Application.Services;
using Abp.Domain.Repositories;
using CROPS.Reports.Contracts;
using CROPS.Reports.DTOs;

namespace CROPS.Reports
{
    public class DashboardAppService : ApplicationService, IDashboardAppService
    {
        private readonly IRepository<Dashboard, string> repository;
        private readonly IReportsProvider reportsProvider;
        //private readonly IRepository<UserFavorite, int> userfavoriterepository;

        public DashboardAppService(
            IRepository<Dashboard, string> repository,
            IReportsProvider reportsProvider)
            //IRepository<UserFavorite, int> userfavoriterepository)
        {
            this.repository = repository;
            this.reportsProvider = reportsProvider;
            //this.userfavoriterepository = userfavoriterepository;
        }

        public DashboardDetailsDto GetDashboard(string id, string userId)
        {
            Dashboard dashboard = repository.Get(id);

            Microsoft.PowerBI.Api.V2.Models.Dashboard result = reportsProvider.GetDashboardAsync(dashboard.WorkspaceId, id).Result;
            //var userfavorite = userfavoriterepository.GetAll().FirstOrDefault(x => x.DashboardId == id && x.UserId == userId);
            //, Isfavorite = userfavorite != null
            DashboardDetailsDto dashboardDTO = new DashboardDetailsDto { DashboardId = result.Id, Name = dashboard.Name, EmbedUrl = result.EmbedUrl };

            return dashboardDTO;
        }
    }
}
