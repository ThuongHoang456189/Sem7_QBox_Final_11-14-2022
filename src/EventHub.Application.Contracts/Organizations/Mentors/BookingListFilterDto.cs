using System;
using Volo.Abp.Application.Dtos;

namespace EventHub.Organizations.Mentors
{
    public class BookingListFilterDto : PagedAndSortedResultRequestDto
    {
        public Guid mentorId { get; set; } 
        public Guid menteeId { get; set; }
        public DateTime minStartTime { get; set; }
    }
}
