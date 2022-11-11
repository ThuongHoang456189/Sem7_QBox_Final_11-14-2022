using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EventHub.Organizations.Mentors.Slots
{
    public interface ISlotRepository : IRepository<Slot, Guid>
    {
        Task<List<SlotWithDetails>> GetListAsync(
            Guid? mentorId = null,
            DateTime? minStartTime = null,
            DateTime? maxStartTime = null,
            CancellationToken cancellationToken = default);
    }
}
