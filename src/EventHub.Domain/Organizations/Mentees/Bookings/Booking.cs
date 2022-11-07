using EventHub.Organizations.Mentors.Slots;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentees.Bookings
{
    public class Booking : FullAuditedAggregateRoot<Guid>
    {
        public Guid SlotId { get; private set; }
        public Slot Slot { get; private set; }

        public Guid MenteeId { get; private set; }
        public Mentee Mentee { get; private set; }

        public byte Status { get; set; }

        public DateTime BookedTime { get; private set; }

        public ICollection<Question> Questions { get; private set; }

        private Booking()
        {

        }

        public Booking(
            Guid id,
            Guid slotId,
            Guid menteeId)
            : base(id)
        {
            SlotId = slotId;
            MenteeId = menteeId;
            Status = (byte) BookingStatus.Created;
            BookedTime = DateTime.Now;
            Questions = new Collection<Question>();
        }

        public Booking AddQuestion(
            Guid questionId,
            string subject,
            string content,
            string directoryRoot)
        {
            if(Questions.Any(x => x.Subject == subject && x.Content == content))
            {
                throw new BusinessException(EventHubErrorCodes.QuestionAlreadyExist)
                    .WithData("Subject", subject);
            }

            Questions.Add(new Question(questionId, Id, subject, content, directoryRoot));

            return this;
        }

        public Booking UpdateQuestion(
            Guid questionId,
            string subject,
            string content,
            string directoryRoot)
        {
            if (Questions.Any(x => x.Subject == subject && x.Content == content && x.Id != questionId))
            {
                throw new BusinessException(EventHubErrorCodes.QuestionAlreadyExist)
                    .WithData("Subject", subject);
            }

            var question = Questions.Single(x => x.Id == questionId);

            question.SetSubject(subject);
            question.SetContent(content);
            question.DirectoryRoot = directoryRoot;

            return this;
        }

        public Booking RemoveQuestion(Guid questionId)
        {
            var question = Questions.SingleOrDefault(x => x.Id == questionId);
            if(question is null)
            {
                throw new BusinessException(EventHubErrorCodes.QuestionNotFound);
            }

            Questions.Remove(question);

            return this;
        }
    }
}