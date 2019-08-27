using Abp.Dependency;
using Abp.Dependency;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Castle.MicroKernel.Registration;
using CROPS.Configuration;
using CROPS.Jobs;
using CROPS.Web.Host.HangFire;
using CROPS.FileSystem;
using CROPS.PowerBI;
using CROPS.Storage;
using CROPS.TFS;
using CROPS.TFSOnline;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CROPS.Web.Host.Startup
{
    [DependsOn(
       typeof(CROPSTFSModule),
       typeof(CROPSWebCoreModule),
       typeof(CROPSPowerBIModule),
       typeof(CROPSFileSystemModule),
       typeof(AbpHangfireAspNetCoreModule),
       typeof(CROPSTFSOnlineModule))]
    public class CROPSWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment env;
        private readonly IConfigurationRoot appConfiguration;

        public CROPSWebHostModule(IHostingEnvironment env)
        {
            this.env = env;
            appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.UseHangfire();
            IocManager.Register<IStorageProvider, FileSystem.FileSystem>(DependencyLifeStyle.Transient);
        }

        public override void PostInitialize()
        {
            // Register Jobs
            HangFireJobsRegisterer hangFireJobsRegisterer = IocManager.Resolve<HangFireJobsRegisterer>();
            hangFireJobsRegisterer.Register();

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CROPSWebHostModule).GetAssembly());
        }
    }
}
