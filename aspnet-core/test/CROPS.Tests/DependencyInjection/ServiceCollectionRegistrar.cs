using System;
using Abp.Dependency;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using CROPS.ALM;
using CROPS.EntityFrameworkCore;
using CROPS.Reports;
using CROPS.HttpClientUtility;
using CROPS.TFS;
using CROPS.TFSOnline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CROPS.Tests.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            services.AddEntityFrameworkInMemoryDatabase();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);

            var builder = new DbContextOptionsBuilder<CROPSDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseInternalServiceProvider(serviceProvider);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<CROPSDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton());

            //iocManager.Register<IReportsProvider, PowerBIProvider>(DependencyLifeStyle.Singleton);


        }
    }
}
