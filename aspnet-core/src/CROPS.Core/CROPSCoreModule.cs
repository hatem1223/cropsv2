using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Timing;
using CROPS.Timing;

namespace CROPS
{
    public class CROPSCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.Languages.Add(new Abp.Localization.LanguageInfo("en-US", "English"));

            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = CROPSConsts.MultiTenancyEnabled;
        }

        public override void Initialize()
        {
            this.IocManager.RegisterAssemblyByConvention(typeof(CROPSCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }
    }
}
