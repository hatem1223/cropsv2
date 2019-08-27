using System.Linq;
using Abp.Authorization;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CROPS.Dtos;
using CROPS.Projects.Contracts;
using CROPS.Projects.DTOs;

namespace CROPS.Projects
{
    [AbpAllowAnonymous]
    public class ProjectDetailsAppService : FilteredCrudAppService<ProjectDetail, ProjectDetailDto>, IProjectDetailsAppService
    {
        public ProjectDetailsAppService(IRepository<ProjectDetail> repository)
            : base(repository)
        {
        }
    }
}
