using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;

namespace EventHub.Organizations.Mentees
{
    public class MenteeAppService : EventHubAppService, IMenteeAppService
    {
        private readonly IRepository<Mentee, Guid> _menteeRepository;

        public MenteeAppService(IRepository<Mentee, Guid> menteeRepository)
        {
            _menteeRepository = menteeRepository;
        }

        [Authorize]
        public async Task AddBooking(Guid slotId)
        {
            var @mentee = await _menteeRepository.GetAsync(CurrentUser.GetId(), true);
            @mentee.AddBooking(GuidGenerator.Create(), slotId);

            await _menteeRepository.UpdateAsync(@mentee);
        }

        [Authorize]
        public async Task RemoveBooking(Guid bookingId)
        {
            var @mentee = await _menteeRepository.GetAsync(CurrentUser.GetId(), true);
            @mentee.RemoveBooking(bookingId);

            await _menteeRepository.UpdateAsync(@mentee);
        }
    }
}
