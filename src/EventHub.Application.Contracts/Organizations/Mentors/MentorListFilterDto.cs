using Volo.Abp.Application.Dtos;

namespace EventHub.Organizations.Mentors
{
    public class MentorListFilterDto : PagedAndSortedResultRequestDto
    {
        public string MajorSubstring { get; set; }

        public string SubjectSubstring { get; set; }
    }
}
