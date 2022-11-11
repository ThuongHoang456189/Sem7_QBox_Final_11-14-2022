using EventHub.Organizations.Mentees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace EventHub.EntityFrameworkCore.Organizations.Mentees
{
    public class MenteeRepository : EfCoreRepository<EventHubDbContext, Mentee, Guid>, IMenteeRepository
    {
        public MenteeRepository(IDbContextProvider<EventHubDbContext> dbContextProvider) : base(dbContextProvider)
        {

        }

        public override async Task<IQueryable<Mentee>> WithDetailsAsync()
        {
            return (await GetQueryableAsync())
                .Include(b => b.Bookings);
        }
    }
}
