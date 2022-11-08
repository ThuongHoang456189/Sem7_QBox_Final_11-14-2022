using System;
using Volo.Abp.Application.Dtos;

namespace EventHub.Organizations.Mentors
{
    public class MentorDto : EntityDto<Guid>
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }

        public string Avatar { get; set; }
    }
}
