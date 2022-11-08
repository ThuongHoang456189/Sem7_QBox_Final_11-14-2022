using System;
using Volo.Abp.Application.Dtos;

namespace EventHub.Organizations.Mentors
{
    public class SlotInListDto : EntityDto<Guid>
    {
        public Guid MentorId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public byte Status { get; set; }
    }
}
