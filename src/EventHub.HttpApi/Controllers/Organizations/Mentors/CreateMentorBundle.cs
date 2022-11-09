using EventHub.Organizations.Mentors;
using Microsoft.AspNetCore.Http;

namespace EventHub.Controllers.Organizations.Mentors
{
    public class CreateMentorBundle
    {
        public CreateMentorBasicInfoDto BasicInfo { get; set; }

        public IFormFile Avatar { get; set; }
    }
}
