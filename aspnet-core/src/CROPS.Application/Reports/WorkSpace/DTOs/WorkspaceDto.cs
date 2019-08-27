using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CROPS.Reports;
using System.Collections.Generic;

namespace CROPS.WorkSpace.DTOs
{
    [AutoMap(typeof(Workspace))]
    public class WorkspaceDto : EntityDto<string>
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public List<DashboardDto> Dashboards { get; set; }

        public List<ReportDto> Reports { get; set; }

    }
}
