using System;

namespace EventHub.Organizations.Mentors
{
    public class SlotListFilterDto
    {
        public Guid mentorId { get; set; }
        public DateTime minStartTime { get; set; }
        public DateTime maxStartTime { get; set; }
    }
}
