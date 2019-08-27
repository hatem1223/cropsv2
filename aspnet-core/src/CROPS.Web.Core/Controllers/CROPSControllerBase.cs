using Abp.AspNetCore.Mvc.Controllers;

namespace CROPS.Controllers
{
    public abstract class CROPSControllerBase : AbpController
    {
        protected CROPSControllerBase()
        {
            LocalizationSourceName = CROPSConsts.LocalizationSourceName;
        }
    }
}
