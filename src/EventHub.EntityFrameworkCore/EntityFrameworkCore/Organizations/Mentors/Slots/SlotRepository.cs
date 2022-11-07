using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using EventHub.Organizations.Mentors.Slots;

namespace EventHub.EntityFrameworkCore.Organizations.Mentors
{
    public class SlotRepository : EfCoreRepository<EventHubDbContext, Slot, Guid>, ISlotRepository
    {

        public SlotRepository(IDbContextProvider<EventHubDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<SlotWithDetails>> GetListAsync(Guid? mentorId = null, DateTime? minStartTime = null, DateTime? maxStartTime = null, byte[] statuses = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = (from @slot in dbContext.Set<Slot>()
                         select new SlotWithDetails
                         {
                             Id = @slot.Id,
                             MentorId = @slot.MentorId,
                             StartTime = @slot.StartTime,
                             EndTime = @slot.EndTime,
                             Status = @slot.Status,
                         })
                        .WhereIf(mentorId.HasValue, x => x.MentorId == mentorId)
                        .WhereIf(minStartTime.HasValue, x => x.StartTime >= minStartTime)
                        .WhereIf(maxStartTime.HasValue, x => x.StartTime <= maxStartTime)
                        .WhereIf(statuses.Length > 0, x => statuses.Any(s => s == ((byte)x.Status)))
                        .OrderBy(x => x.StartTime);

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
