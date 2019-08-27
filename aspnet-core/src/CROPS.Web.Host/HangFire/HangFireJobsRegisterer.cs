using Abp.Dependency;
using CROPS.Jobs;
using CROPS.Web.Host.HangFire.ConfigModels;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CROPS.Web.Host.HangFire
{
    public class HangFireJobsRegisterer : IHangFireJobsRegisterer, ITransientDependency
    {
        private readonly IConfiguration appConfig;

        private HangFireJobsConfig HangFireJobsConfig { get; set; }

        public HangFireJobsRegisterer(IOptions<HangFireJobsConfig> settings)
        {
            HangFireJobsConfig = settings.Value;
        }

        public void Register()
        {
            //RecurringJob.AddOrUpdate("InsertDataFromPowerBiRestApiIntoDatabaseJob", () => IocManager.Instance.Resolve<IInsertDataFromPowerBiRestApiIntoDatabaseJob>().Execute().Wait(), HangFireJobsConfig.InsertDataFromPowerBiRestApiIntoDatabaseWorkerCronExpression);
        }
    }
}
