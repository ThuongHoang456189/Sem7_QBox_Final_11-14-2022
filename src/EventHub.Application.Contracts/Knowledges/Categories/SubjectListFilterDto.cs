using Volo.Abp.Application.Dtos;

namespace EventHub.Knowledges.Categories
{
    public class SubjectListFilterDto : PagedAndSortedResultRequestDto
    {
        public string DisplaySubstring { get; set; }
    }
}