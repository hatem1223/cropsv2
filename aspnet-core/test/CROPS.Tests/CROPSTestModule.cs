using System;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Castle.MicroKernel.Registration;
using CROPS.ALM;
using CROPS.EntityFrameworkCore;
using CROPS.HttpClientUtility;
using CROPS.Tests.DependencyInjection;
using CROPS.Tests.Mock;
using Microsoft.Extensions.Configuration;
using Moq;
using CROPS.TFS;
using CROPS.TFSOnline;
using NSubstitute;

namespace CROPS.Tests
{
    [DependsOn(
        typeof(CROPSApplicationModule),
        typeof(CROPSEntityFrameworkModule),
        typeof(AbpTestBaseModule))]
    public class CROPSTestModule : AbpModule
    {
        public CROPSTestModule(CROPSEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            // Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            //RegisterFakeService<IHttpClientManager>();
            //Configuration.ReplaceService<IHttpClientManager, NullHttpClientManager>(DependencyLifeStyle.Singleton);
            Configuration.ReplaceService<IConfiguration, ConfigurationMock>();
            //Mock<IHttpClientManager>();
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        /*private void RegisterFakeService<TService>()
            where TService : class
        {
            var mock = new Mock<TService>();

            IocManager.IocContainer.Register(
                Component.For<Mock<TService>>()
                    .UsingFactoryMethod(() => mock)
                    .LifestyleSingleton());

            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => new Mock<TService>().Object)
                    .LifestyleSingleton());
        }

        private void Mock<TBase>()
            where TBase : class
        {
            IocManager.IocContainer.Register(
                Component.For<TBase>().UsingFactoryMethod(() =>
                {
                    var mockObject = new Mock<TBase>();
                    var mockType = typeof(TBase);

                    if (IocManager.IocContainer is Core.IoCContainer.IoCCore.CustomContainer)
                    {
                        var resolvingDelege = (IocManager.IocContainer as Core.IoCContainer.IoCCore.CustomContainer).ResolvingTestDelegateList[mockType];

                        if (resolvingDelege != null)
                        {
                            resolvingDelege(mockType, mockObject);
                        }
                    }

                    //The code that was changed
                    //==============================================
                    var type = mockObject.Object.GetType();
                    var field = type.GetField("__target");

                    field.SetValue(mockObject.Object, new Object());
                    //==============================================


                    return mockObject.Object;
                })
            .LifestyleTransient());
        }*/
    }
}
