using Abp.Application.Services;
using CROPS.Reports.DTOs;
using CROPS.WorkSpace.DTOs;
using System.Collections.Generic;

namespace CROPS.Reports.Contracts
{
    public interface IWorkspaceAppService : IApplicationService
    {
        List<WorkspaceDto> GetAll();
    }
}
