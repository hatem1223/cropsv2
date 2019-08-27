using Abp.Application.Services;
using CROPS.Dtos;
using CROPS.Projects.DTOs;

namespace CROPS.Projects.Contracts
{
    public interface IProjectAppService : IAsyncCrudAppService<DTOs.ProjectDto, int, FilteredResultRequestDto>
    {
    }
}
