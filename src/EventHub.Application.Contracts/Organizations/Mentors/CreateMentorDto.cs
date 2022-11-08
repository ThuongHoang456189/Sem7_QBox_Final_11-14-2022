using EventHub.Organizations.Mentees;
using System;
using System.ComponentModel.DataAnnotations;

namespace EventHub.Organizations.Mentors
{
    public class CreateMentorDto
    {
        [Required]
        [StringLength(MenteeConsts.MaxEmailLength)]
        public string Email { get; set; }

        [Required]
        [StringLength(MentorConsts.MaxNameLength, MinimumLength = MentorConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(MentorConsts.MaxPhoneNumberLength, MinimumLength = MentorConsts.MinPhoneNumberLength)]
        public string PhoneNumber { get; set; }
    }
}
