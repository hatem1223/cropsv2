using System;
using System.Threading.Tasks;
using Abp;
using Abp.TestBase;
using Castle.MicroKernel.Registration;
using CROPS.EntityFrameworkCore;
using Castle.MicroKernel.Registration;

namespace CROPS.Tests
{
    public abstract class CROPSTestBase : AbpIntegratedTestBase<CROPSTestModule>
    {
        protected CROPSTestBase()
        {
            void NormalizeDbContext(CROPSDbContext context)
            {
            }

            // Seed initial data for host
            AbpSession.TenantId = null;
            UsingDbContext(context =>
            {
                NormalizeDbContext(context);
            });

            // Seed initial data for default tenant
            AbpSession.TenantId = 1;
            UsingDbContext(context =>
            {
                NormalizeDbContext(context);
            });
        }

        protected IDisposable UsingTenantId(int? tenantId)
        {
            var previousTenantId = AbpSession.TenantId;
            AbpSession.TenantId = tenantId;
            return new DisposeAction(() => AbpSession.TenantId = previousTenantId);
        }

        protected void UsingDbContext(Action<CROPSDbContext> action)
        {
            UsingDbContext(AbpSession.TenantId, action);
        }

        protected Task UsingDbContextAsync(Func<CROPSDbContext, Task> action)
        {
            return UsingDbContextAsync(AbpSession.TenantId, action);
        }

        protected T UsingDbContext<T>(Func<CROPSDbContext, T> func)
        {
            return UsingDbContext(AbpSession.TenantId, func);
        }

        protected Task<T> UsingDbContextAsync<T>(Func<CROPSDbContext, Task<T>> func)
        {
            return UsingDbContextAsync(AbpSession.TenantId, func);
        }

        protected void UsingDbContext(int? tenantId, Action<CROPSDbContext> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CROPSDbContext>())
                {
                    action(context);
                    context.SaveChanges();
                }
            }
        }

        protected async Task UsingDbContextAsync(int? tenantId, Func<CROPSDbContext, Task> action)
        {
            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CROPSDbContext>())
                {
                    await action(context).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        protected T UsingDbContext<T>(int? tenantId, Func<CROPSDbContext, T> func)
        {
            T result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CROPSDbContext>())
                {
                    result = func(context);
                    context.SaveChanges();
                }
            }

            return result;
        }

        protected async Task<T> UsingDbContextAsync<T>(int? tenantId, Func<CROPSDbContext, Task<T>> func)
        {
            T result;

            using (UsingTenantId(tenantId))
            {
                using (var context = LocalIocManager.Resolve<CROPSDbContext>())
                {
                    result = await func(context).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }

            return result;
        }

        protected void RegisterInstance<T>(T instance)
            where T : class
        {
            LocalIocManager.IocContainer.Register(
                Component.For<T>()
                .Instance(instance)
                .Named(Guid.NewGuid().ToString())
                .IsDefault());
        }
    }
}
