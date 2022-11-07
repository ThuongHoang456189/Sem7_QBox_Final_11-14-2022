using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace EventHub.Knowledges.Categories
{
    public interface ISubjectRepository : IRepository<Subject, Guid>
    {
        Task<int> GetCountAsync(
            string displaySubstring = null,
            CancellationToken cancellationToken = default);

        Task<List<SubjectWithDetails>> GetListAsync(
            string sorting = null,
            int skipCount = 0,
            int maxResultCount = int.MaxValue,
            string displaySubstring = null,
            CancellationToken cancellationToken = default);
    }
}
