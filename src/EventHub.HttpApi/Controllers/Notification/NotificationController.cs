using EventHub.Notification;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace EventHub.Controllers.Notification
{
    public class NotificationController : AbpController
    {
        private readonly INotificationAppService _notificationAppService;

        public NotificationController(INotificationAppService notificationAppService)
        {
            _notificationAppService = notificationAppService;
        }

        [HttpPost]
        [Route("push-notification")]
        public async Task<IActionResult> SendNotification(NotificationDto notificationModel)
        {
            var result = await _notificationAppService.SendNotification(notificationModel);
            return Ok(result);
        }
    }
}
