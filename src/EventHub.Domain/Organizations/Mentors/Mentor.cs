using EventHub.Organizations.Mentors.Profiles;
using EventHub.Organizations.Mentors.Slots;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentors
{
    public class Mentor : FullAuditedAggregateRoot<Guid>
    {
        public string Email { get; private set; }

        public string Name { get; private set; }

        public DateTime DateOfBirth { get; private set; }

        public string PhoneNumber { get; private set; }

        public string Avatar { get; private set; }

        public ICollection<MentorSkill> MentorSkills { get; private set; }

        public ICollection<Slot> Slots { get; private set; }

        private Mentor()
        {

        }

        public Mentor(
            Guid id,
            string email,
            string name,
            DateTime dateOfBirth,
            string phoneNumber,
            string avatar)
            : base(id)
        {
            SetEmail(email);
            SetName(name);
            SetDateOfBirth(dateOfBirth);
            SetPhoneNumber(phoneNumber);
            SetAvatar(avatar);
            MentorSkills = new Collection<MentorSkill>();
            Slots = new Collection<Slot>();

        }

        public Mentor SetEmail(string email)
        {
            Email = Check.NotNullOrWhiteSpace(email, nameof(email), MentorConsts.MaxEmailLength);
            return this;
        }

        public Mentor SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), MentorConsts.MaxNameLength);
            return this;
        }

        public Mentor SetDateOfBirth(DateTime dateOfBirth)
        {
            if (dateOfBirth.AddYears(MentorConsts.MinAge) > DateTime.Now)
            {
                throw new BusinessException(EventHubErrorCodes.MentorShouldOlderThanMinAge);
            }

            DateOfBirth = dateOfBirth;
            return this;
        }

        public Mentor SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = Check.NotNullOrWhiteSpace(phoneNumber, nameof(phoneNumber), MentorConsts.MaxPhoneNumberLength, MentorConsts.MinPhoneNumberLength);
            return this;
        }

        public Mentor SetAvatar(string avatar)
        {
            Avatar = avatar.IsNullOrWhiteSpace() ? MentorConsts.DefaultAvatar : avatar;
            return this;
        }

        public Mentor AddMentorSkill(
            Guid mentorSkillId,
            Guid subjectId,
            string title,
            string description)
        {
            if(MentorSkills.Any(x => x.Title == title))
            {
                throw new BusinessException(EventHubErrorCodes.MentorSkillAlreadyExist)
                    .WithData("Skill", title);
            }

            MentorSkills.Add(new MentorSkill(mentorSkillId,Id,subjectId,title,description));

            return this;
        }

        public Mentor UpdateMentorSkill(
            Guid mentorSkillId,
            Guid subjectId,
            string title,
            string description)
        {
            if (MentorSkills.Any(x => x.Title == title && x.Id != mentorSkillId))
            {
                throw new BusinessException(EventHubErrorCodes.MentorSkillAlreadyExist)
                    .WithData("Skill", title);
            }

            var mentorSkill = MentorSkills.Single(x => x.Id == mentorSkillId);
            mentorSkill.SubjectId = subjectId;
            mentorSkill.SetTitle(title);
            mentorSkill.SetDescription(description);

            return this;
        }

        public Mentor DeleteMentorSkill(Guid mentorSkillId)
        {
            var mentorSkill = MentorSkills.SingleOrDefault(x => x.Id == mentorSkillId);
            if(mentorSkill is null)
            {
                throw new BusinessException(EventHubErrorCodes.MentorSkillNotFound);
            }

            MentorSkills.Remove(mentorSkill);
            return this;
        }

        private MentorSkill GetMentorSkill(Guid Id)
        {
            return MentorSkills.FirstOrDefault(x => x.Id == Id) ??
                throw new EntityNotFoundException(typeof(MentorSkill), Id);
        }

        public Mentor AddCertificate(
            Guid mentorSkillId,
            Guid certificateId,
            string issuer,
            string accomplishment,
            DateTime issuanceDate,
            string directoryRoot)
        {
            if (issuanceDate <= DateOfBirth.AddYears(MentorConsts.MinAge))
            {
                throw new BusinessException(EventHubErrorCodes.CertificateIssuanceDateShouldBeAfterMinAge);
            }

            var mentorSkill = GetMentorSkill(mentorSkillId);

            mentorSkill.AddCertificate(certificateId, issuer, accomplishment, issuanceDate, directoryRoot);
            return this;
        }

        public Mentor UpdateCertificate(
            Guid mentorSkillId,
            Guid certificateId,
            string issuer,
            string accomplishment,
            DateTime issuanceDate,
            string directoryRoot)
        { 
            if(issuanceDate <= DateOfBirth.AddYears(MentorConsts.MinAge)) 
            {
                throw new BusinessException(EventHubErrorCodes.CertificateIssuanceDateShouldBeAfterMinAge);
            }

            var mentorSkill = GetMentorSkill(mentorSkillId);

            mentorSkill.UpdateCertificate(certificateId,issuer,accomplishment,issuanceDate,directoryRoot);
            return this;
        }

        public Mentor DeleteCertificate(Guid mentorSkillId, Guid certificateId)
        {
            var mentorSkill = MentorSkills.SingleOrDefault(x => x.Id == mentorSkillId);

            if(mentorSkill is null)
            {
                throw new BusinessException(EventHubErrorCodes.MentorSkillNotFound);
            }

            mentorSkill.DeleteCertificate(certificateId);
            return this;
        }

        private bool CheckTwoIntersectTimeIntervals(DateTime startTime1, DateTime endTime1, DateTime startTime2, DateTime endTime2)
        {
            return startTime1 <= endTime2 && startTime2 <= endTime1;        
        }

        public Mentor AddSlot(
            Guid slotId,
            DateTime startTime,
            DateTime endTime)
        {
            if (startTime < DateTime.Now.AddMinutes(SlotConsts.MinTimeBetweenStartTimeAndNowInMinute))
            {
                throw new BusinessException(EventHubErrorCodes.StartTimeCantBeEarlierThanNowPlusAmountOfTime)
                    .WithData("MinTimeBetweenStartTimeAndNowInMinute", SlotConsts.MinTimeBetweenStartTimeAndNowInMinute);
            }

            if (startTime > endTime.AddMinutes(SlotConsts.MinSlotInternalInMinute))
            {
                throw new BusinessException(EventHubErrorCodes.EndTimeCantBeEarlierThanStartTimePlusAmountOfTime)
                    .WithData("MinSlotInternalInMinute", SlotConsts.MinSlotInternalInMinute);
            }

            if (Slots.Any(x => CheckTwoIntersectTimeIntervals(x.StartTime, x.EndTime, startTime, endTime)))
            {
                throw new BusinessException(EventHubErrorCodes.SlotIntervalsIntersect);
            }

            Slots.Add(new Slot(slotId, Id, startTime, endTime, (byte) SlotStatus.Open));
            return this;
        }

        public Mentor UpdateSlot(
            Guid slotId,
            DateTime startTime,
            DateTime endTime,
            byte status)
        {
            if (startTime < DateTime.Now.AddMinutes(SlotConsts.MinTimeBetweenStartTimeAndNowInMinute))
            {
                throw new BusinessException(EventHubErrorCodes.StartTimeCantBeEarlierThanNowPlusAmountOfTime)
                    .WithData("MinTimeBetweenStartTimeAndNowInMinute", SlotConsts.MinTimeBetweenStartTimeAndNowInMinute);
            }

            if (startTime > endTime.AddMinutes(SlotConsts.MinSlotInternalInMinute))
            {
                throw new BusinessException(EventHubErrorCodes.EndTimeCantBeEarlierThanStartTimePlusAmountOfTime)
                    .WithData("MinSlotInternalInMinute", SlotConsts.MinSlotInternalInMinute);
            }

            if (Slots.Any(x => CheckTwoIntersectTimeIntervals(x.StartTime, x.EndTime, startTime, endTime) && x.Id != slotId))
            {
                throw new BusinessException(EventHubErrorCodes.SlotIntervalsIntersect);
            }

            var slot = Slots.Single(x => x.Id == slotId);

            slot.SetTime(startTime, endTime);
            slot.Status = status;

            return this;
        }

        public Mentor RemoveSlot(Guid slotId)
        {
            var slot = Slots.SingleOrDefault(x => x.Id == slotId);
            if(slot is null)
            {
                throw new BusinessException(EventHubErrorCodes.SlotNotFound);
            }

            Slots.Remove(slot);

            return this;
        }
    }
}