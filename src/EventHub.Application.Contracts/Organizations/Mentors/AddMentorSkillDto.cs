using System;

namespace EventHub.Organizations.Mentors
{
    public class AddMentorSkillDto
    {
        public Guid SubjectId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
