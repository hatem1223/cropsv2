using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace CROPS.Reports.DTOs
{
    [AutoMap(typeof(Report))]
    public class ReportDetailsDto : EntityDto<string>
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string ReportId { get; set; }
    }
}
