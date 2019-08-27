using System.Collections.Generic;
using Abp.Application.Services;
using CROPS.Reports.DTOs;

namespace CROPS.Reports.Contracts
{
    public interface IReportAppService : IApplicationService
    {
        ReportDetailsDto Get(string id);
    }
}
