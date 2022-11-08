using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;

namespace EventHub.BlobContainer
{
    public class QBoxFileAppService : ApplicationService, IQBoxFileAppService
    {
        private readonly IBlobContainer<QBoxFileContainer> _fileContainer;

        public QBoxFileAppService(IBlobContainer<QBoxFileContainer> fileContainer)
        {
            _fileContainer = fileContainer;
        }

        public async Task<BlobDto> GetBlobAsync(GetBlobRequestDto input)
        {
            var blob = await _fileContainer.GetAllBytesAsync(input.Name);

            return new BlobDto
            {
                Name = input.Name,
                File = blob
            };
        }

        public async Task SaveBlobAsync(SaveBlobInputDto input)
        {
            await _fileContainer.SaveAsync(input.Name, input.File, true);
        }
    }
}
