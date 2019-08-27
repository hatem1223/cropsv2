using Abp.Application.Services;
using CROPS.Dtos;
using CROPS.Projects.DTOs;

namespace CROPS.Projects.Contracts
{
    public interface IProjectDetailsAppService : IAsyncCrudAppService<ProjectDetailDto, int, FilteredResultRequestDto>
    {
    }
}
