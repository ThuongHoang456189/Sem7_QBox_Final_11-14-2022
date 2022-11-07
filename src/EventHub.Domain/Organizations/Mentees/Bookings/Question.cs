using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentees.Bookings
{
    public class Question : FullAuditedAggregateRoot<Guid>
    {
        public Guid BookingId { get; private set; }
        public Booking Booking { get; private set; }

        public string Subject { get; private set; }

        public string Content { get; private set; }

        public string DirectoryRoot { get; set; }

        private Question()
        {

        }

        public Question(
            Guid id,
            Guid bookingId,
            string subject,
            string content,
            string directoryRoot)
            : base(id)
        {
            BookingId = bookingId;
            SetSubject(subject);
            SetContent(content);
            DirectoryRoot = directoryRoot;
        }

        public Question SetSubject(string subject)
        {
            Subject = Check.NotNullOrWhiteSpace(subject, nameof(subject), BookingConsts.MaxSubjectLength);
            return this;
        }

        public Question SetContent(string content)
        {
            Content = Check.NotNullOrWhiteSpace(content, nameof(content), BookingConsts.MaxContentLength);
            return this;
        }
    }
}