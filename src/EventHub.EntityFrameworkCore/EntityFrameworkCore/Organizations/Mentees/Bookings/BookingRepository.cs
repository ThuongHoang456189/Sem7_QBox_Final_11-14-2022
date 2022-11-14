using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using EventHub.Organizations.Mentees.Bookings;
using EventHub.Organizations.Mentors.Slots;
using EventHub.Organizations.Mentees;
using Microsoft.EntityFrameworkCore;

namespace EventHub.EntityFrameworkCore.Organizations.Mentors
{
    public class BookingRepository : EfCoreRepository<EventHubDbContext, Booking, Guid>, IBookingRepository
    {

        public BookingRepository(IDbContextProvider<EventHubDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<int> GetCountAsync(
            Guid? mentorId = null,  
            DateTime? minStartTime = null, 
            CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = (from @booking in dbContext.Set<Booking>()
                         join slot in dbContext.Set<Slot>() on @booking.SlotId equals slot.Id
                         join mentee in dbContext.Set<Mentee>() on @booking.MenteeId equals mentee.Id
                         select new BookingWithDetails
                         {
                             BookingId = @booking.Id,
                             Slot = new SlotWithDetails
                             {
                                 Id = slot.Id,
                                 MentorId = slot.MentorId,
                                 StartTime = slot.StartTime,
                                 EndTime = slot.EndTime,
                                 Status = slot.Status
                             },
                             MenteeId = @booking.MenteeId,
                             MenteeName = mentee.Name,
                             Status = @booking.Status,
                             BookedTime = @booking.BookedTime
                         })
                        .WhereIf(mentorId.HasValue, x => x.Slot.MentorId == mentorId)
                        //.WhereIf(menteeId.HasValue, x => x.MenteeId == menteeId)
                        .WhereIf(minStartTime.HasValue, x => x.Slot.StartTime >= minStartTime);

            return await query.CountAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<BookingWithDetails>> GetListAsync(string sorting = null, int skipCount = 0, int maxResultCount = int.MaxValue, Guid? mentorId = null, DateTime? minStartTime = null, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            var query = (from @booking in dbContext.Set<Booking>()
                         join slot in dbContext.Set<Slot>() on @booking.SlotId equals slot.Id
                         join mentee in dbContext.Set<Mentee>() on @booking.MenteeId equals mentee.Id
                         select new BookingWithDetails
                         {
                             BookingId = @booking.Id,
                             Slot = new SlotWithDetails
                             {
                                 Id = slot.Id,
                                 MentorId = slot.MentorId,
                                 StartTime = slot.StartTime,
                                 EndTime = slot.EndTime,
                                 Status = slot.Status
                             },
                             MenteeId = @booking.MenteeId,
                             MenteeName = mentee.Name,
                             Status = @booking.Status,
                             BookedTime = @booking.BookedTime
                         })
                        .WhereIf(mentorId.HasValue, x => x.Slot.MentorId == mentorId)
                        //.WhereIf(menteeId.HasValue, x => x.MenteeId == menteeId)
                        .WhereIf(minStartTime.HasValue, x => x.Slot.StartTime >= minStartTime)
                        .OrderBy(x => x.BookedTime)
                        .PageBy(skipCount, maxResultCount);

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
