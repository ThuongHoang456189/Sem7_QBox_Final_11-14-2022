using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using EventHub.Knowledges.Categories;

namespace EventHub.EntityFrameworkCore.Knowledges.Categories
{
    public class SubjectRepository : EfCoreRepository<EventHubDbContext, Subject, Guid>, ISubjectRepository
    {
        public SubjectRepository(IDbContextProvider<EventHubDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<int> GetCountAsync(string displaySubstring = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = (from @subject in dbContext.Set<Subject>()
                         join major in dbContext.Set<Major>() on @subject.MajorId equals major.Id
                         select new SubjectWithDetails
                         {
                             Id = @subject.Id,
                             Title = @subject.Title,
                             Major = new MajorWithDetails
                             {
                                 Id = major.Id,
                                 Title = major.Title
                             }
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(displaySubstring), x => x.Title.ToLower().Contains(displaySubstring) || x.Major.Title.ToLower().Contains(displaySubstring));

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<SubjectWithDetails>> GetListAsync(string sorting = null, int skipCount = 0, int maxResultCount = int.MaxValue, string displaySubstring = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = (from @subject in dbContext.Set<Subject>()
                         join major in dbContext.Set<Major>() on @subject.MajorId equals major.Id
                         select new SubjectWithDetails
                         {
                             Id = @subject.Id,
                             Title = @subject.Title,
                             Major = new MajorWithDetails
                             {
                                 Id = major.Id,
                                 Title = major.Title
                             }
                         })
                         .WhereIf(!string.IsNullOrWhiteSpace(displaySubstring), x => x.Title.ToLower().Contains(displaySubstring) || x.Major.Title.ToLower().Contains(displaySubstring))
                         .OrderBy(string.IsNullOrWhiteSpace(sorting) ? SubjectConsts.DefaultSorting : sorting)
                         .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
