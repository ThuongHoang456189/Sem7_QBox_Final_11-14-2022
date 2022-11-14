using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace EventHub.Organizations.Mentors
{
    public interface IMentorAppService : IApplicationService
    {
        Task<MentorDto> CreateAsync(CreateMentorBasicInfoDto input, string avatarExtension, byte[] MentorAvatar);

        Task<PagedResultDto<MentorInListDto>> GetMentorsBySubjectAsync(MentorListFilterDto input);

        Task<List<SlotInListDto>> GetSlotsAsync(SlotListFilterDto input);

        Task AddSlotAsync(AddSlotDto input);

        Task AddMentorSkillAsync(AddMentorSkillDto input);

        Task UpdateSlotAsync(Guid id, UpdateSlotDto input);

        Task DeleteSlotAsync(Guid id);

        Task<PagedResultDto<BookingInListDto>> GetBookingsAsync(BookingListFilterDto input);

        Task AcceptBookingAsync(Guid slotId, Guid bookingId);

        Task DenyBookingAsync(Guid slotId, Guid bookingId);
    }
}
