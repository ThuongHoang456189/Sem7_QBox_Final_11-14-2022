using System.ComponentModel.DataAnnotations;

namespace EventHub.BlobContainer
{
    public class GetBlobRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
