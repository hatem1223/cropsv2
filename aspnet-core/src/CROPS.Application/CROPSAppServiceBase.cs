using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Runtime.Session;

namespace CROPS
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class CROPSAppServiceBase : ApplicationService
    {
        protected CROPSAppServiceBase()
        {
            LocalizationSourceName = CROPSConsts.LocalizationSourceName;
        }
    }
}
