using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EventHub.Knowledges.Categories
{
    public interface ISubjectAppService : IApplicationService
    {
        Task<PagedResultDto<SubjectLookupDto>> GetSubjectsLookupAsync(SubjectListFilterDto input);
    }
}