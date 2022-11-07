using EventHub.Organizations.Mentees.Bookings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentors.Slots
{
    public class Slot : FullAuditedAggregateRoot<Guid>
    {
        public Guid MentorId { get; private set; }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public byte Status { get; set; }

        public ICollection<Booking> Bookings { get; private set; }

        private Slot()
        {

        }

        public Slot(
            Guid id,
            Guid mentorId,
            DateTime startTime,
            DateTime endTime,
            byte status)
            : base(id)
        {
            MentorId = mentorId;
            SetTime(startTime, endTime);
            Status = status;
            Bookings = new Collection<Booking>();
        }

        public Slot SetTime(DateTime startTime, DateTime endTime)
        {
            if (startTime == StartTime && endTime == EndTime)
            {
                return this;
            }

            if (startTime < DateTime.Now.AddMinutes(SlotConsts.MinTimeBetweenStartTimeAndNowInMinute))
            {
                throw new BusinessException(EventHubErrorCodes.StartTimeCantBeEarlierThanNowPlusAmountOfTime)
                    .WithData("MinTimeBetweenStartTimeAndNowInMinute", SlotConsts.MinTimeBetweenStartTimeAndNowInMinute);
            }

            SetTimeInterval(startTime, endTime);

            return this;
        }

        private Slot SetTimeInterval(DateTime startTime, DateTime endTime)
        {
            if (startTime > endTime.AddMinutes(SlotConsts.MinSlotInternalInMinute))
            {
                throw new BusinessException(EventHubErrorCodes.EndTimeCantBeEarlierThanStartTimePlusAmountOfTime)
                    .WithData("MinSlotInternalInMinute", SlotConsts.MinSlotInternalInMinute);
            }

            StartTime = startTime;
            EndTime = endTime;
            return this;
        }

        public Slot AddBooking(
            Guid bookingId,
            Guid menteeId)
        {
            if(Status == 1)
            {
                throw new BusinessException(EventHubErrorCodes.SlotClosed);
            }

            if(Status == 2)
            {
                throw new BusinessException(EventHubErrorCodes.SlotFullyBooked);
            }

            if(Bookings.Any(x => x.MenteeId == menteeId && (x.Status == (byte) BookingStatus.Created || x.Status == (byte) BookingStatus.Accepted || x.Status == (byte)BookingStatus.Denied)))
            {
                throw new BusinessException(EventHubErrorCodes.BookingRequestAlreadyCreated);
            }

            Bookings.Add(new Booking(bookingId, Id, menteeId));

            if (Status == 0 && (Bookings.Count(x => x.Status == (byte) BookingStatus.Accepted) == SlotConsts.MaxAcceptedMentees))
            {
                Status = 2;
            }

            return this;
        }

        public Slot UpdateBooking(
            Guid bookingId,
            byte status)
        {
            var booking = Bookings.Single(x => x.Id == bookingId);

            if(booking.Status == (byte) BookingStatus.Invalid || booking.Status == status)
            {
                return this;
            }

            booking.Status = status;
            return this;
        }

        public Slot RemoveBooking(Guid bookingId)
        {
            var booking = Bookings.SingleOrDefault(x => x.Id == bookingId);
            if(booking is null)
            {
                throw new BusinessException(EventHubErrorCodes.BookingNotFound);
            }

            Bookings.Remove(booking);
            return this;
        }
    }
}