using System;
using Volo.Abp.Application.Dtos;

namespace EventHub.Organizations.Mentors
{
    public class BookingInListDto : EntityDto<Guid>
    {
        public Guid MenteeId { get; set; }

        public string MenteeName { get; set; }

        public byte Status { get; set; }

        public DateTime BookedTime { get; set; }
    }
}
