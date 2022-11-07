using System;

namespace EventHub.Organizations.Mentors.Slots
{
    public class SlotWithDetails
    {
        public Guid Id { get; set; }

        public Guid MentorId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public byte Status { get; set; }
    }
}
