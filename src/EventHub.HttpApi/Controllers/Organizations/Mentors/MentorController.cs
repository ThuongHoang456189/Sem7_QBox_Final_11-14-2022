using EventHub.BlobContainer;
using EventHub.Organizations.Mentors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace EventHub.Controllers.Organizations.Mentors
{
    [RemoteService(Name = EventHubRemoteServiceConsts.QBoxRemoteServiceName)]
    [Area("qbox")]
    [ControllerName("Mentor")]
    [Route("api/qbox/mentor")]
    public class MentorController : AbpController
    {
        private readonly IMentorAppService _mentorAppService;
        private readonly IQBoxFileAppService _fileAppService;
        private const string MentorAvatarRoot = "mentor-avatars";

        public MentorController(IMentorAppService mentorAppService, IQBoxFileAppService fileAppService)
        {
            _mentorAppService = mentorAppService;
            _fileAppService = fileAppService;
        }

        [HttpPost]
        [Route("profile/create-profile")]
        public async Task<MentorDto> CreateAsync(CreateMentorBasicInfoDto input, [FromForm] IFormFile avatar)
        {
            byte[] mentorAvatar = {};
            if (avatar != null && avatar.Length > 0)
            {
                var memoryStream = new MemoryStream();
                await avatar.CopyToAsync(memoryStream);
                mentorAvatar = memoryStream.ToArray();
            }
            return await _mentorAppService.CreateAsync(input, mentorAvatar);
        }

        [HttpGet("profile")]
        public async Task<PagedResultDto<MentorInListDto>> GetListAsync(MentorListFilterDto input)
        {
            return await _mentorAppService.GetMentorsBySubjectAsync(input);
        }

        [HttpGet]
        [Route("profile/avatar/{file}")]
        public async Task<IActionResult> GetAvatarAsync(string file)
        {
            file = string.IsNullOrWhiteSpace(file) ? MentorConsts.DefaultAvatar : file;

            var fileDto = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name = file });

            return File(fileDto.File, "application/octet-stream", fileDto.Name);
        }

        [HttpGet]
        [Route(("slots"))]
        public async Task<List<SlotInListDto>> GetSlotsAsync(SlotListFilterDto input)
        {
            return await _mentorAppService.GetSlotsAsync(input);
        }

        [HttpPost]
        [Route("slots/create")]
        public async Task AddSlotAsync(AddSlotDto input)
        {
            await _mentorAppService.AddSlotAsync(input);
        }

        [HttpPost]
        [Route("slots/{id}")]
        public async Task UpdateSlotAsync(Guid id, UpdateSlotDto input)
        {
            await _mentorAppService.UpdateSlotAsync(id, input);
        }

        [HttpDelete]
        [Route("slots/{id}")]
        public async Task DeleteSlotAsync(Guid id)
        {
            await _mentorAppService.DeleteSlotAsync(id);
        }

        [HttpGet]
        [Route("bookings")]
        public async Task<PagedResultDto<BookingInListDto>> GetBookingsAsync(BookingListFilterDto input)
        {
            return await _mentorAppService.GetBookingsAsync(input);
        }

        [HttpPost]
        [Route("slots/{slotId}/bookings/{bookingId}/accept")]
        public async Task AcceptBookingAsync(Guid slotId, Guid bookingId)
        {
            await _mentorAppService.AcceptBookingAsync(slotId, bookingId);
        }

        [HttpPost]
        [Route("slots/{slotId}/bookings/{bookingId}/deny")]
        public async Task DenyBookingAsync(Guid slotId, Guid bookingId)
        {
            await _mentorAppService.DenyBookingAsync(slotId, bookingId);
        }
    }
}
