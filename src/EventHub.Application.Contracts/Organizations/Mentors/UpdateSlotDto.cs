using System;

namespace EventHub.Organizations.Mentors
{
    public class UpdateSlotDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public byte Status { get; set; }
    }
}
