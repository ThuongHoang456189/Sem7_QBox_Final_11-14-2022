using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Knowledges.Categories
{
    public class Major : FullAuditedAggregateRoot<Guid>
    {
        public string Title { get; private set; }

        public string Description { get; private set; }

        public ICollection<Subject> Subjects { get; private set; }

        private Major()
        {

        }

        internal Major(
            Guid id,
            string title,
            string description)
            : base(id)
        {
            setTitle(title);
            setDescription(description);
            Subjects = new Collection<Subject>();
        }

        public Major setTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), MajorConsts.MaxTitleLength);
            return this;
        }

        public Major setDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), MajorConsts.MaxDescriptionLength);
            return this;
        }

        internal Major AddSubject(
            Guid subjectId,
            string title,
            string description)
        {
            if (Subjects.Any(s => s.Title == title))
            {
                throw new BusinessException(EventHubErrorCodes.MajorTitleAlreadyExist)
                    .WithData("Title", title);
            }

            Subjects.Add(new Subject(subjectId, Id, title, description));

            return this;
        }

        internal Major UpdateSubject(
            Guid subjectId,
            string title,
            string description)
        {
            if (Subjects.Any(s => s.Title == title && s.Id != subjectId))
            {
                throw new BusinessException(EventHubErrorCodes.MajorTitleAlreadyExist)
                    .WithData("Title", title);
            }

            var subject = Subjects.Single(s => s.Id == subjectId);

            subject.SetTitle(title);
            subject.SetDescription(description);

            return this;
        }

        internal Major RemoveSubject(Guid subjectId)
        {
            var subject = Subjects.SingleOrDefault(s => s.Id == subjectId);
            if (subject is null)
            {
                throw new BusinessException(EventHubErrorCodes.SubjectNotFound);
            }

            Subjects.Remove(subject);

            return this;
        }
    }
}