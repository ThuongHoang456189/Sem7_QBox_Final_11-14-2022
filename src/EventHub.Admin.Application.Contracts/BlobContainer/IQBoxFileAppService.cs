using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EventHub.BlobContainer
{
    public interface IQBoxFileAppService : IApplicationService
    {
        Task SaveBlobAsync(SaveBlobInputDto input);

        Task<BlobDto> GetBlobAsync(GetBlobRequestDto input);
    }
}