using System;

namespace EventHub.Organizations.Mentors
{
    public class CreateMentorBasicInfoDto
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string PhoneNumber { get; set; }
    }
}
