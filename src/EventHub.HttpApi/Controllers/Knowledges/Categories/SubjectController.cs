using EventHub.Knowledges.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace EventHub.Controllers.Knowledges.Categories
{
    [RemoteService(Name = EventHubRemoteServiceConsts.QBoxRemoteServiceName)]
    [Area("qbox")]
    [ControllerName("Subject")]
    [Route("api/qbox/subject")]
    public class SubjectController : AbpController
    {
        private readonly ISubjectAppService _subjectAppService;

        public SubjectController(ISubjectAppService subjectAppService)
        {
            _subjectAppService = subjectAppService;
        }

        [HttpGet]
        public async Task<PagedResultDto<SubjectLookupDto>> GetListAsync(SubjectListFilterDto input)
        {
            return await _subjectAppService.GetSubjectsLookupAsync(input);
        }
    }
}
