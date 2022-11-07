using System;

namespace EventHub.Knowledges.Categories
{
    public class SubjectWithDetails
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public MajorWithDetails Major { get; set; }
    }
}
