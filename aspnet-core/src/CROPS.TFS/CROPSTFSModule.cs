using Abp.Modules;
using Castle.MicroKernel.Registration;
using CROPS.ALM;

namespace CROPS.TFS
{
    [DependsOn(
        typeof(CROPSCoreModule))]
    public class CROPSTFSModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.IocContainer.Register(Component.For<IAlmProvider>().ImplementedBy<TFSProvider>().Named("TFS"));
        }
    }
}
