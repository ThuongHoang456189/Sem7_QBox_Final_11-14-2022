using EventHub.Countries;
using EventHub.Events;
using EventHub.Events.Registrations;
using EventHub.Knowledges.Categories;
using EventHub.Organizations;
using EventHub.Organizations.Memberships;
using EventHub.Organizations.Mentees;
using EventHub.Organizations.Mentees.Bookings;
using EventHub.Organizations.Mentors;
using EventHub.Organizations.Mentors.Profiles;
using EventHub.Organizations.Mentors.Slots;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Identity;

namespace EventHub.EntityFrameworkCore
{
    public static class EventHubDbContextModelCreatingExtensions
    {
        public static void ConfigureEventHub(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            builder.Entity<Organization>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "Organizations", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(OrganizationConsts.MaxNameLength);
                b.Property(x => x.DisplayName).IsRequired().HasMaxLength(OrganizationConsts.MaxDisplayNameLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(OrganizationConsts.MaxDescriptionNameLength);
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.OwnerUserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

                b.HasIndex(x => x.Name);
                b.HasIndex(x => x.DisplayName);
            });
            
            builder.Entity<OrganizationMembership>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "OrganizationMemberships", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.HasOne<Organization>().WithMany().HasForeignKey(x => x.OrganizationId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

                b.HasIndex(x => new {x.OrganizationId, x.UserId});
            });

            builder.Entity<Event>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "Events", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Title).IsRequired().HasMaxLength(EventConsts.MaxTitleLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(EventConsts.MaxDescriptionLength);
                b.Property(x => x.UrlCode).IsRequired().HasMaxLength(EventConsts.UrlCodeLength);
                b.Property(x => x.Url).IsRequired().HasMaxLength(EventConsts.MaxUrlLength);
                b.Property(x => x.OnlineLink).HasMaxLength(EventConsts.MaxOnlineLinkLength);
                b.Property(x => x.City).HasMaxLength(EventConsts.MaxCityLength);
                b.Property(x => x.Language).HasMaxLength(EventConsts.MaxLanguageLength);

                b.HasOne<Organization>().WithMany().HasForeignKey(x => x.OrganizationId).IsRequired().OnDelete(DeleteBehavior.NoAction);
               
                b.HasOne<Country>().WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.NoAction);

                b.Property(x => x.IsTimingChangeEmailSent).HasDefaultValue(true);

                b.HasIndex(x => new {x.OrganizationId, x.StartTime});
                b.HasIndex(x => x.StartTime);
                b.HasIndex(x => x.UrlCode);
                b.HasIndex(x => new {x.IsRemindingEmailSent, x.StartTime});
                b.HasIndex(x => x.IsEmailSentToMembers);

                b.HasMany(x => x.Tracks).WithOne().IsRequired().HasForeignKey(x => x.EventId);
            });

            builder.Entity<Track>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "EventTracks", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(TrackConsts.MaxNameLength);
                
                b.HasMany(x => x.Sessions).WithOne().IsRequired().HasForeignKey(x => x.TrackId);
            });
            
            builder.Entity<Session>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "EventSessions", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Title).IsRequired().HasMaxLength(SessionConsts.MaxTitleLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SessionConsts.MaxDescriptionLength);
                b.Property(x => x.Language).IsRequired().HasMaxLength(SessionConsts.MaxLanguageLength);
                
                b.HasMany(x => x.Speakers).WithOne().IsRequired().HasForeignKey(x => x.SessionId);
            });
            
            builder.Entity<Speaker>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "EventSpeakers", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.HasKey(x => new {x.SessionId, x.UserId});

                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
            });

            builder.Entity<EventRegistration>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "EventRegistrations", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.HasOne<Event>().WithMany().HasForeignKey(x => x.EventId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired().OnDelete(DeleteBehavior.NoAction);

                b.HasIndex(x => new {x.EventId, x.UserId});
            });
            
            builder.Entity<Country>(b =>
            {
                b.ToTable(EventHubConsts.DbTablePrefix + "Countries", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Name).IsRequired().HasMaxLength(CountryConsts.MaxNameLength);

                b.HasIndex(x => new {x.Name});
            });

            // QBox Table
            builder.Entity<Major>(b =>
            {
                b.ToTable("Major", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Title).IsRequired().HasMaxLength(MajorConsts.MaxTitleLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(MajorConsts.MaxDescriptionLength);

                b.HasIndex(x => new { x.Title });
            });

            builder.Entity<Subject>(b =>
            {
                b.ToTable("Subject", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.HasOne<Major>().WithMany().HasForeignKey(x => x.MajorId).IsRequired().OnDelete(DeleteBehavior.NoAction);
                b.Property(x => x.Title).IsRequired().HasMaxLength(SubjectConsts.MaxTitleLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(SubjectConsts.MaxDescriptionLength);

                b.HasIndex(x => new { x.Title });
            });

            builder.Entity<Mentor>(b =>
            {
                b.ToTable<Mentor>("Mentor", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Email).IsRequired().HasMaxLength(MentorConsts.MaxEmailLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(MentorConsts.MaxNameLength);
                b.Property(x => x.DateOfBirth).IsRequired();
                b.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(MentorConsts.MaxPhoneNumberLength);
                b.Property(x => x.Avatar).IsRequired().HasMaxLength(MentorConsts.MaxAvatarLength);

                b.HasMany(x => x.MentorSkills).WithOne().IsRequired().HasForeignKey(x => x.MentorId);
                b.HasMany(x => x.Slots).WithOne().IsRequired().HasForeignKey(x => x.MentorId);

                b.HasIndex(x => new { x.Email }).IsUnique();
            });

            builder.Entity<MentorSkill>(b =>
            {
                b.ToTable("MentorSkill", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Title).IsRequired().HasMaxLength(MentorSkillConsts.MaxTitleLength);
                b.Property(x => x.Description).IsRequired().HasMaxLength(MentorSkillConsts.MaxDescriptionLength);
                b.Property(x => x.IsQBoxApprovedSkill).HasDefaultValue(false);
                b.HasMany<Certificate>().WithOne().IsRequired().HasForeignKey(x => x.MentorSkillId);

                b.HasOne<Subject>().WithMany().HasForeignKey(x => x.SubjectId).OnDelete(DeleteBehavior.NoAction);
            });

            builder.Entity<Certificate>(b =>
            {
                b.ToTable("Certificate", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Issuer).IsRequired().HasMaxLength(CertificateConsts.MaxIssuerLength);
                b.Property(x => x.Accomplishment).IsRequired().HasMaxLength(CertificateConsts.MaxAccomplishmentLength);
                b.Property(x => x.IssuanceDate).IsRequired();
                b.Property(x => x.DirectoryRoot).IsRequired().HasMaxLength(CertificateConsts.MaxDirectoryRootLength);

                b.HasIndex(x => new { x.Accomplishment, x.Issuer }).IsUnique();
            });

            builder.Entity<Slot>(b =>
            {
                b.ToTable("Slot", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.StartTime).IsRequired();
                b.Property(x => x.EndTime).IsRequired();
                b.Property(x => x.Status);
            });

            builder.Entity<Mentee>(b =>
            {
                b.ToTable("Mentee", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.Property(x => x.Email).IsRequired().HasMaxLength(MenteeConsts.MaxEmailLength);
                b.Property(x => x.Name).IsRequired().HasMaxLength(MenteeConsts.MaxNameLength);
                b.Property(x => x.DateOfBirth);
                b.Property(x => x.PhoneNumber).HasMaxLength(MenteeConsts.MaxPhoneNumberLength);
                b.Property(x => x.Avatar).IsRequired().HasMaxLength(MentorConsts.MaxAvatarLength);

                b.HasIndex(x => new { x.Email }).IsUnique();
            });

            builder.Entity<Booking>(b =>
            {
                b.ToTable("Booking", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.HasOne<Slot>().WithMany().HasForeignKey(x => x.SlotId).OnDelete(DeleteBehavior.NoAction);
                b.HasOne<Mentee>().WithMany().HasForeignKey(x => x.MenteeId).OnDelete(DeleteBehavior.NoAction);
                b.Property(x => x.Status).IsRequired();
                b.Property(x => x.BookedTime).IsRequired();

                b.HasIndex(x => new { x.SlotId, x.MenteeId, x.Status }).IsUnique();
            });

            builder.Entity<Question>(b =>
            {
                b.ToTable("Question", EventHubConsts.DbSchema);

                b.ConfigureByConvention();

                b.HasOne<Booking>().WithMany().HasForeignKey(x => x.BookingId).OnDelete(DeleteBehavior.NoAction);
                b.Property(x => x.Subject).IsRequired().HasMaxLength(BookingConsts.MaxSubjectLength);
                b.Property(x => x.Content).IsRequired().HasMaxLength(BookingConsts.MaxContentLength);
                b.Property(x => x.DirectoryRoot).IsRequired().HasMaxLength(BookingConsts.MaxDirectoryRootLength);
            });

        }
    }
}
