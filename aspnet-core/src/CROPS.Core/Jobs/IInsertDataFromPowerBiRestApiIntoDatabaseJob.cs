using Abp.Domain.Services;
using System.Threading.Tasks;

namespace CROPS.Jobs
{
    public interface IInsertDataFromPowerBiRestApiIntoDatabaseJob : IDomainService
    {
        string Id { get; set; }

        Task Execute();
    }
}