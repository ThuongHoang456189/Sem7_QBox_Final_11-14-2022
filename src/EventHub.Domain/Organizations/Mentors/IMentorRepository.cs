using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EventHub.Organizations.Mentors
{
    public interface IMentorRepository : IRepository<Mentor, Guid>
    {
        Task<int> GetCountAsync(
            string majorTitle = null,
            string subjectTitle = null,
            CancellationToken cancellationToken = default);

        Task<List<MentorWithDetails>> GetListAsync(
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            string majorTitle = null,
            string subjectTitle = null,
            CancellationToken cancellationToken = default);
    }
}
