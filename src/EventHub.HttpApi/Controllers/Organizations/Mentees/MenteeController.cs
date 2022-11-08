using EventHub.Organizations.Mentees;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace EventHub.Controllers.Organizations.Mentees
{
    [RemoteService(Name = EventHubRemoteServiceConsts.QBoxRemoteServiceName)]
    [Area("qbox")]
    [ControllerName("Mentee")]
    [Route("api/qbox/mentee")]
    public class MenteeController
    {
        private readonly IMenteeAppService _menteeAppService;

        public MenteeController(IMenteeAppService menteeAppService)
        {
            _menteeAppService = menteeAppService;
        }

        [HttpPost]
        [Route("bookings/slot/{slotId}/add")]
        public async Task AddBooking(Guid slotId)
        {
            await _menteeAppService.AddBooking(slotId);
        }

        [HttpPost]
        [Route("bookings/{bookingId}/add")]
        public async Task RemoveBooking(Guid bookingId)
        {
            await _menteeAppService.RemoveBooking(bookingId);
        }
    }
}
