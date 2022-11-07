using EventHub.Organizations.Mentors.Profiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Knowledges.Categories
{
    public class Subject : FullAuditedAggregateRoot<Guid>
    {
        public Guid MajorId { get; private set; }
        public Major Major { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public ICollection<MentorSkill> MentorSkills { get; private set; }

        internal Subject(
            Guid id,
            Guid majorId,
            string title,
            string description)
            : base(id)
        {
            MajorId = majorId;
            SetTitle(title);
            SetDescription(description);
            MentorSkills = new Collection<MentorSkill>();
        }

        public Subject SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), SubjectConsts.MaxTitleLength);
            return this;
        }

        public Subject SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), SubjectConsts.MaxDescriptionLength);
            return this;
        }

        public Subject AddMentorSkill(
            Guid mentorSkillId,
            Guid mentorId,
            string title,
            string description)
        {
            MentorSkills.Add(new MentorSkill(mentorSkillId, mentorId, Id, title, description));

            return this;
        }

        public Subject UpdateMentorSkill(
            Guid mentorSkillId,
            Guid mentorId,
            string title,
            string description)
        {
            var mentorSkill = MentorSkills.Single(x => x.Id == mentorSkillId);
            mentorSkill.SetTitle(title);
            mentorSkill.SetDescription(description);

            return this;
        }

        public Subject DeleteMentorSkill(Guid mentorSkillId)
        {
            var mentorSkill = MentorSkills.SingleOrDefault(x => x.Id == mentorSkillId);
            if (mentorSkill is null)
            {
                throw new BusinessException(EventHubErrorCodes.MentorSkillNotFound);
            }

            MentorSkills.Remove(mentorSkill);
            return this;
        }
    }
}