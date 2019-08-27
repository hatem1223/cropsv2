using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace CROPS.EntityFrameworkCore
{
    [DependsOn(
        typeof(CROPSCoreModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class CROPSEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<CROPSDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        CROPSDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        CROPSDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(CROPSEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
