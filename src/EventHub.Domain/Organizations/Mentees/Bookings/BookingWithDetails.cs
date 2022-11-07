using EventHub.Organizations.Mentors.Slots;
using System;

namespace EventHub.Organizations.Mentees.Bookings
{
    public class BookingWithDetails
    {
        public Guid BookingId { get; set; }

        public SlotWithDetails Slot { get; set; }

        public Guid MenteeId { get; set; }

        public string MenteeName { get; set; }

        public byte Status { get; set; }

        public DateTime BookedTime { get; set; }
    }
}
