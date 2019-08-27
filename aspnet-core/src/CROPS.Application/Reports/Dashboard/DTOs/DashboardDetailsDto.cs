using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace CROPS.Reports.DTOs
{
    [AutoMap(typeof(Dashboard))]
    public class DashboardDetailsDto : EntityDto<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
        public string EmbedUrl { get; set; }

        public string DashboardId { get; set; }

        public bool Isfavorite { get; set; }

        public string WorkspaceId { get; set; }

    }
}
