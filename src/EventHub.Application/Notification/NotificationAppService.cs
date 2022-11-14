using CorePush.Google;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using static EventHub.Notification.GoogleNotificationDto;

namespace EventHub.Notification
{
    public class NotificationAppService : EventHubAppService, INotificationAppService
    {
        public async Task<ResponseDto> SendNotification(NotificationDto notificationDto)
        {
            ResponseDto response = new();
            try
            {
                FcmSettings settings = new()
                {
                    SenderId = "429061642069",
                    ServerKey = "AAAAY-YRF1U:APA91bGMNw5fi5_MOw2r58DcmG1CsTo6uw7XF406BR43EweO8BpJsDfxIKGVeYGuCWdy4SukooS_A5emJZzBhkqwuh-U0dRc8sqkHJKqbledRAwVgr9SB8kYqfP-356zNF0gRd2kSRpZ"
                };
                HttpClient httpClient = new HttpClient();
                string authorizationKey = string.Format("key={0}", settings.ServerKey);
                string deviceToken = notificationDto.DeviceId;
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorizationKey);
                httpClient.DefaultRequestHeaders.Accept
                        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                DataPayload dataPayload = new();
                dataPayload.Title = notificationDto.Title;
                dataPayload.Body = notificationDto.Body;
                GoogleNotificationDto notification = new GoogleNotificationDto();
                notification.Data = dataPayload;
                notification.Notification = dataPayload;
                var fcm = new FcmSender(settings, httpClient);
                var fcmSendResponse = await fcm.SendAsync(deviceToken, notification);
                if (fcmSendResponse.IsSuccess())
                {
                    response.IsSuccess = true;
                    response.Message = "Notification sent successfully";
                    return response;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = fcmSendResponse.Results[0].Error;
                    return response;
                }
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "Exception";
                return response;
            }
        }
    }
}
