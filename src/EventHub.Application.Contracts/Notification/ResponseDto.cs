using Newtonsoft.Json;

namespace EventHub.Notification
{
    public class ResponseDto
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
