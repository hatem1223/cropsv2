using System.Threading.Tasks;
using Abp.Domain.Repositories;
using CROPS.Projects;

namespace CROPS.EntityFrameworkCore.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<SP_GetProjectDataByProjectAreaPath_Result> GetProjectDataByProjectAreaPath(string projectAreaPath);
    }
}
