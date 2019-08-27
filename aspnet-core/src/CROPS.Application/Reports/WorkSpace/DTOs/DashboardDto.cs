using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CROPS.Reports;

namespace CROPS.WorkSpace.DTOs
{
    [AutoMap(typeof(Dashboard))]
    public class DashboardDto : EntityDto<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string DashboardId { get; set; }
    }
}
