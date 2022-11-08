using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace EventHub.Knowledges.Categories
{
    public class SubjectAppService : EventHubAppService, ISubjectAppService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectAppService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository;
        }

        [Authorize]
        public async Task<PagedResultDto<SubjectLookupDto>> GetSubjectsLookupAsync(SubjectListFilterDto input)
        {
            var totalCount = await _subjectRepository.GetCountAsync(
                input.DisplaySubstring);

            var items = await _subjectRepository.GetListAsync(
                input.Sorting,
                input.SkipCount,
                input.MaxResultCount,
                input.DisplaySubstring);

            var subjects = ObjectMapper.Map<List<SubjectWithDetails>, List<SubjectLookupDto>>(items);

            return new PagedResultDto<SubjectLookupDto>(totalCount, subjects);
        }
    }
}
