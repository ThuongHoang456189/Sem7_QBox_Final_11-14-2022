using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentors.Profiles
{
    // 1st phase just use Entity<Guid> but then can be changed to FullAuditedAggregateRoot<Guid>
    public class Certificate : FullAuditedAggregateRoot<Guid>
    {
        public Guid MentorSkillId { get; private set; }
        public MentorSkill MentorSkill { get; private set; }

        public string Issuer { get; private set; }

        public string Accomplishment { get; private set; }

        public DateTime IssuanceDate { get; private set; }

        public string DirectoryRoot { get; set; }

        private Certificate()
        {

        }

        public Certificate(
            Guid id,
            Guid mentorSkillId,
            string issuer,
            string accomplishment,
            DateTime issuanceDate,
            string directoryRoot)
            : base(id)
        {
            MentorSkillId = mentorSkillId;
            SetIssuer(issuer);
            SetAccomplishment(accomplishment);
            SetIssuanceDate(issuanceDate);
            DirectoryRoot = directoryRoot;
        }

        public Certificate SetIssuer(string issuer)
        {
            Issuer = Check.NotNullOrWhiteSpace(issuer, nameof(issuer), CertificateConsts.MaxIssuerLength);
            return this;
        }

        public Certificate SetAccomplishment(string accomplishment)
        {
            Accomplishment = Check.NotNullOrWhiteSpace(accomplishment, nameof(accomplishment), CertificateConsts.MaxAccomplishmentLength);
            return this;
        }

        public Certificate SetIssuanceDate(DateTime issuanceDate)
        {
            // This place can be changed if multiple regions apply
            if(issuanceDate >= DateTime.Now)
            {
                throw new BusinessException(EventHubErrorCodes.CertificateIssuanceDateShouldBeEarlierThanNow);
            }

            IssuanceDate = issuanceDate;
            return this;
        }
    }
}