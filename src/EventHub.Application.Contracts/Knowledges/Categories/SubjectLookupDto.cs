using System;
using Volo.Abp.Application.Dtos;

namespace EventHub.Knowledges.Categories
{
    public class SubjectLookupDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string MajorTitle { get; set; }
    }
}