using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CROPS.Reports;

namespace CROPS.WorkSpace.DTOs
{
    [AutoMap(typeof(Report))]
    public class ReportDto : EntityDto<string>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ReportId { get; set; }
    }
}
