using EventHub.Knowledges.Categories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace EventHub.Organizations.Mentors.Profiles
{
    // Cho nay phai la quan he composite with mentor
    public class MentorSkill : FullAuditedAggregateRoot<Guid>
    {
        public Guid MentorId { get; private set; }

        public Guid SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        // For 1st phase, default is true, but then need to implement admin page and approve page, then default false
        public bool IsQBoxApprovedSkill { get; private set; } = false;

        public ICollection<Certificate> Certificates { get; private set; }

        public MentorSkill(
            Guid id,
            Guid mentorId,
            Guid subjectId,
            string title,
            string description)
            : base(id)
        {
            MentorId = mentorId;
            SubjectId = subjectId;
            SetTitle(title);
            SetDescription(description);
            Certificates = new Collection<Certificate>();
        }

        public MentorSkill SetTitle(string title)
        {
            Title = Check.NotNullOrWhiteSpace(title, nameof(title), MentorSkillConsts.MaxTitleLength);
            return this;
        }

        public MentorSkill SetDescription(string description)
        {
            Description = Check.NotNullOrWhiteSpace(description, nameof(description), MentorSkillConsts.MaxDescriptionLength);
            return this;
        }

        public MentorSkill ApproveMentorSkill()
        {
            IsQBoxApprovedSkill = true;
            return this;
        }

        public MentorSkill AddCertificate(
            Guid certificateId,
            string issuer,
            string accomplishment,
            DateTime issuanceDate,
            string directoryRoot)
        {
            if(Certificates.Any(x => x.Issuer == issuer && x.Accomplishment == accomplishment))
            {
                throw new BusinessException(EventHubErrorCodes.CertificateAlreadyExist)
                    .WithData("Certificate", accomplishment);
            }

            Certificates.Add(new Certificate(certificateId, Id, issuer, accomplishment, issuanceDate, directoryRoot));

            return this;
        }

        public MentorSkill UpdateCertificate(
            Guid certificateId,
            string issuer,
            string accomplishment,
            DateTime issuanceDate,
            string directoryRoot)
        {
            if (Certificates.Any(x => x.Issuer == issuer && x.Accomplishment == accomplishment && x.Id != certificateId))
            {
                throw new BusinessException(EventHubErrorCodes.CertificateAlreadyExist)
                    .WithData("Accomplishment", accomplishment)
                    .WithData("Issuer", issuer);
            }

            var certificate = Certificates.Single(x => x.Id == certificateId);

            certificate.SetIssuer(issuer);
            certificate.SetAccomplishment(accomplishment);
            certificate.SetIssuanceDate(issuanceDate);
            certificate.DirectoryRoot = directoryRoot;

            return this;
        }

        public MentorSkill DeleteCertificate(Guid certificateId)
        {
            var certificate = Certificates.SingleOrDefault(x => x.Id == certificateId);
            if(certificate is null)
            {
                throw new BusinessException(EventHubErrorCodes.CertificateNotFound);
            }

            Certificates.Remove(certificate);
            return this;
        }
    }
}