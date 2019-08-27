using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.SignalR;
using Abp.Modules;
using Abp.Reflection.Extensions;
using CROPS.Configuration;
using CROPS.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace CROPS
{
    [DependsOn(
         typeof(CROPSApplicationModule),
         typeof(CROPSEntityFrameworkModule),
         typeof(AbpAspNetCoreModule),
         typeof(AbpAspNetCoreSignalRModule))]
    public class CROPSWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment env;
        private readonly IConfigurationRoot appConfiguration;

        public CROPSWebCoreModule(IHostingEnvironment env)
        {
            this.env = env;
            appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = appConfiguration.GetConnectionString(
                CROPSConsts.ConnectionStringName);

            Configuration.Modules.AbpAspNetCore()
                 .CreateControllersForAppServices(
                     typeof(CROPSApplicationModule).GetAssembly());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CROPSWebCoreModule).GetAssembly());
        }
    }
}
