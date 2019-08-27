using System.Threading.Tasks;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using CROPS.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CROPS.Web.Host.Controllers
{
    public class HomeController : CROPSControllerBase
    {
        private readonly INotificationPublisher notificationPublisher;

        public HomeController(INotificationPublisher notificationPublisher)
        {
            this.notificationPublisher = notificationPublisher;
        }

        public IActionResult Index()
        {
            return Redirect("/swagger/index.html");
        }

        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!.
        /// </summary>
        /// <param name="message">Notification message content.</param>
        /// <returns>ActionResult.</returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }).ConfigureAwait(false);

            return Content("Sent notification: " + message);
        }
    }
}
