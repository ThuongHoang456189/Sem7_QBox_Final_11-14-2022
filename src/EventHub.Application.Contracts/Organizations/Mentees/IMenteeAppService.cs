using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace EventHub.Organizations.Mentees
{
    public interface IMenteeAppService : IApplicationService
    {
        Task AddBooking(Guid slotId);

        Task RemoveBooking(Guid bookingId);
    }
}
