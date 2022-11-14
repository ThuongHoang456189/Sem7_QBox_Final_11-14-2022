using System.Threading.Tasks;

namespace EventHub.Notification
{
    public interface INotificationAppService
    {
        Task<ResponseDto> SendNotification(NotificationDto notificationModel);
    }
}
