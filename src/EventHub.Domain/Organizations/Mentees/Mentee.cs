using EventHub.Organizations.Mentees.Bookings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentees
{
    public class Mentee : FullAuditedAggregateRoot<Guid>
    {
        public string Email { get; private set; }

        public string Name { get; private set; }

        public DateTime? DateOfBirth { get; private set; }

        public string? PhoneNumber { get; private set; }

        public string Avatar { get; private set; }

        public ICollection<Booking> Bookings { get; private set; }

        private Mentee()
        {

        }

        public Mentee(
            Guid id,
            string email,
            string name,
            DateTime? dateOfBirth,
            string? phoneNumber,
            string avatar)
            : base(id)
        {
            SetEmail(email);
            Name = name;
            SetDateOfBirth(dateOfBirth);
            //SetPhoneNumber(phoneNumber);
            PhoneNumber = phoneNumber;
            SetAvatar(avatar);

            Bookings = new Collection<Booking>();
        }

        public Mentee SetEmail(string email)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), MenteeConsts.MaxEmailLength);
            return this;
        }

        public Mentee SetName(string name)
        {
            name = String.IsNullOrWhiteSpace(name) ? Email : Name;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MenteeConsts.MaxNameLength);
            return this;
        }

        public Mentee SetDateOfBirth(DateTime? dateOfBirth)
        {
            if (dateOfBirth?.AddYears(MenteeConsts.MinAge) > DateTime.Now)
            {
                throw new BusinessException(EventHubErrorCodes.MentorShouldOlderThanMinAge);
            }

            DateOfBirth = dateOfBirth;
            return this;
        }

        //public Mentee SetPhoneNumber(string phoneNumber)
        //{
        //    PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), MenteeConsts.MaxPhoneNumberLength, MenteeConsts.MinPhoneNumberLength);
        //    return this;
        //}

        public Mentee SetAvatar(string avatar)
        {
            Avatar = avatar.IsNullOrWhiteSpace() ? MenteeConsts.DefaultAvatar : avatar;
            return this;
        }

        public Mentee AddBooking(
            Guid bookingId,
            Guid slotId)
        {
            if(Bookings.Any(x => x.SlotId == slotId))
            {
                throw new BusinessException(EventHubErrorCodes.BookingRequestAlreadyCreated);
            }

            Bookings.Add(new Booking(bookingId, slotId, Id));
            return this;
        }

        public Mentee RemoveBooking(Guid bookingId)
        {
            var booking = Bookings.SingleOrDefault(x => x.Id == bookingId);
            if (booking is null)
            {
                throw new BusinessException(EventHubErrorCodes.BookingNotFound);
            }

            Bookings.Remove(booking);
            return this;
        }
    }
}