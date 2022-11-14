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
        public async Task<MentorDto> CreateAsync([FromForm]  CreateMentorBundle input)
        {
            byte[] mentorAvatar = {};
            if (input.Avatar != null && input.Avatar.Length > 0)
            {
                var memoryStream = new MemoryStream();
                await input.Avatar.CopyToAsync(memoryStream);
                mentorAvatar = memoryStream.ToArray();
            }

            string avatarExtension = Path.GetExtension(input.Avatar.FileName);
            return await _mentorAppService.CreateAsync(input.BasicInfo, avatarExtension, mentorAvatar);
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
            file = file.Replace(@"%2F", @"/");
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
        [Route("mentorskills/create")]
        public async Task AddMentorSkillAsync(AddMentorSkillDto input)
        {
            await _mentorAppService.AddMentorSkillAsync(input);
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
