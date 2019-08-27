using Abp.Modules;
using Castle.MicroKernel.Registration;
using CROPS.ALM;

namespace CROPS.TFSOnline
{
    [DependsOn(
        typeof(CROPSCoreModule))]
    public class CROPSTFSOnlineModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.IocContainer.Register(Component.For<IAlmProvider>().ImplementedBy<TFSOnlineProvider>().Named("TFSOnline"));
        }
    }
}
