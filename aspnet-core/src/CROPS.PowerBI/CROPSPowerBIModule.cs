using Abp.Modules;
using CROPS.Reports;

namespace CROPS.PowerBI
{
    [DependsOn(
        typeof(CROPSCoreModule))]
    public class CROPSPowerBIModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.Register<IReportsProvider, PowerBIProvider>(Abp.Dependency.DependencyLifeStyle.Transient);
        }
    }
}
