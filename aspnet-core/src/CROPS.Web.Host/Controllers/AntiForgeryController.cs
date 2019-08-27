using CROPS.Controllers;
using Microsoft.AspNetCore.Antiforgery;

namespace CROPS.Web.Host.Controllers
{
    public class AntiForgeryController : CROPSControllerBase
    {
        private readonly IAntiforgery antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            this.antiforgery = antiforgery;
        }

        public void GetToken()
        {
            antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
