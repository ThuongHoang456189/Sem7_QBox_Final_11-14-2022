using System;
using Volo.Abp.Application.Dtos;

namespace EventHub.Organizations.Mentors
{
    public class MentorInListDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Avatar { get; set; }
    }
}
