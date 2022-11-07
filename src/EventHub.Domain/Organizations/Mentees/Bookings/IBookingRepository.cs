using EventHub.Organizations.Mentors.Slots;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EventHub.Organizations.Mentees.Bookings
{
    public interface IBookingRepository : IRepository<Booking, Guid>
    {
        Task<int> GetCountAsync(
            Guid? mentorId = null,
            Guid? menteeId = null,
            DateTime? minStartTime = null,
            byte[] statuses = null,
            CancellationToken cancellationToken = default);

        Task<List<BookingWithDetails>> GetListAsync(
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            Guid? mentorId = null,
            Guid? menteeId = null,
            DateTime? minStartTime = null,
            byte[] statuses = null,
            CancellationToken cancellationToken = default);
    }
}
