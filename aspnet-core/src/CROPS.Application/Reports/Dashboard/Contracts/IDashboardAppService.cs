using Abp.Application.Services;
using CROPS.Reports.DTOs;

namespace CROPS.Reports.Contracts
{
    public interface IDashboardAppService : IApplicationService
    {
        DashboardDetailsDto GetDashboard(string id, string userId);
    }
}
