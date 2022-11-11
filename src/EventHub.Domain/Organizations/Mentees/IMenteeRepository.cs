using System;
using Volo.Abp.Domain.Repositories;

namespace EventHub.Organizations.Mentees
{
    public interface IMenteeRepository : IRepository<Mentee, Guid>
    {
    }
}
