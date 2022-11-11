using EventHub.Knowledges.Categories;
using EventHub.Organizations.Mentors;
using EventHub.Organizations.Mentors.Profiles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace EventHub.EntityFrameworkCore.Organizations.Mentors
{
    public class MentorRepository : EfCoreRepository<EventHubDbContext, Mentor, Guid>, IMentorRepository
    {

        public MentorRepository(IDbContextProvider<EventHubDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        private class SubjectFound
        {
            internal Guid SubjectId { get; set; }
            internal string MajorTitle { get; set; }
            internal string SubjectTitle { get; set; }
        }

        private class MentorFound
        {
            internal Guid MentorId { get; set; }
        }

        public async Task<List<MentorWithDetails>> GetListAsync(
            string sorting = null, 
            int skipCount = 0, 
            int maxResultCount = int.MaxValue, 
            string majorTitle = null, 
            string subjectTitle = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            IQueryable<SubjectFound> subjectQueryable = (from @subject in dbContext.Set<Subject>()
                                    join major in dbContext.Set<Major>() on @subject.MajorId equals major.Id
                                    select new SubjectFound
                                    {
                                        SubjectId = @subject.Id,
                                        MajorTitle = major.Title,
                                        SubjectTitle = @subject.Title
                                    })
                                    .WhereIf(!string.IsNullOrWhiteSpace(majorTitle) || !string.IsNullOrWhiteSpace(subjectTitle), x => x.MajorTitle.ToLower().Contains(majorTitle.ToLower()) || x.SubjectTitle.ToLower().Contains(subjectTitle.ToLower()));

            IQueryable<MentorFound> mentorSkillQueryable = (from @mentorSkill in dbContext.Set<MentorSkill>()
                                        join subject in subjectQueryable on @mentorSkill.SubjectId equals subject.SubjectId
                                        group @mentorSkill by @mentorSkill.MentorId
                                        into mentor
                                        select new MentorFound
                                        {
                                            MentorId = mentor.Key
                                        });

            var query = (from @mentor in dbContext.Set<Mentor>()
                         join mentorSkill in mentorSkillQueryable on @mentor.Id equals mentorSkill.MentorId
                         select new MentorWithDetails
                         {
                             Id = @mentor.Id,
                             Name = @mentor.Name,
                             Avatar = @mentor.Avatar
                         })
                         .OrderBy(string.IsNullOrWhiteSpace(sorting) ? MentorConsts.DefaultSorting : sorting)
                         .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<int> GetCountAsync(string majorTitle = null, string subjectTitle = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            IQueryable<SubjectFound> subjectQueryable = (from @subject in dbContext.Set<Subject>()
                                    join major in dbContext.Set<Major>() on @subject.MajorId equals major.Id
                                    select new SubjectFound
                                    {
                                        SubjectId = @subject.Id,
                                        MajorTitle = major.Title,
                                        SubjectTitle = @subject.Title
                                    })
                                    .WhereIf(!string.IsNullOrWhiteSpace(majorTitle) || !string.IsNullOrWhiteSpace(subjectTitle), x => x.MajorTitle.ToLower().Contains(majorTitle.ToLower()) || x.SubjectTitle.ToLower().Contains(subjectTitle.ToLower()));

            IQueryable<MentorFound> mentorSkillQueryable = (from @mentorSkill in dbContext.Set<MentorSkill>()
                                        join subject in subjectQueryable on @mentorSkill.SubjectId equals subject.SubjectId
                                        group @mentorSkill by @mentorSkill.MentorId
                                        into mentor
                                        select new MentorFound
                                        {
                                            MentorId = mentor.Key
                                        });

            var query = (from @mentor in dbContext.Set<Mentor>()
                         join mentorSkill in mentorSkillQueryable on @mentor.Id equals mentorSkill.MentorId
                         select new MentorWithDetails
                         {
                             Id = @mentor.Id,
                             Name = @mentor.Name,
                             Avatar = @mentor.Avatar
                         });

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public override async Task<IQueryable<Mentor>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(ms => ms.MentorSkills)
                .Include(s => s.Slots)
                .ThenInclude(b => b.Bookings);
        }
    }
}
