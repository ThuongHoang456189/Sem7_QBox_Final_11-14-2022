using System.ComponentModel.DataAnnotations;

namespace EventHub.BlobContainer
{
    public class SaveBlobInputDto
    {
        public byte[] File { get; set; }

        [Required]
        public string Name { get; set; }
    }
}