using EventHub.Admin.Permissions;
using EventHub.BlobContainer;
using EventHub.Organizations.Mentees.Bookings;
using EventHub.Organizations.Mentors.Slots;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace EventHub.Organizations.Mentors
{
    public class MentorAppService : EventHubAppService, IMentorAppService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IMentorRepository _mentorRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IQBoxFileAppService _qBoxFileAppService;
        
        private const string MentorAvatarRoot = "mentor-avatars";

        public MentorAppService(IPermissionManager permissionManager, IMentorRepository mentorRepository, ISlotRepository slotRepository, IBookingRepository bookingRepository, IQBoxFileAppService qBoxFileAppService)
        {
            _permissionManager = permissionManager;
            _mentorRepository = mentorRepository;
            _slotRepository = slotRepository;
            _bookingRepository = bookingRepository;
            _qBoxFileAppService = qBoxFileAppService;
        }

        [Authorize]
        public async Task<MentorDto> CreateAsync(CreateMentorBasicInfoDto input, string avatarExtension, byte[] MentorAvatar)
        {
            // Check if exists
            var isMentorExisted = await IsMentorExisted();

            input.Email = String.IsNullOrWhiteSpace(input.Email) ? CurrentUser.Email : input.Email;
            input.Name = String.IsNullOrWhiteSpace(input.Name) ? CurrentUser.Name : input.Name;
            input.PhoneNumber = String.IsNullOrWhiteSpace(input.PhoneNumber) ? CurrentUser.PhoneNumber : input.PhoneNumber;

            if (isMentorExisted)
            {
                throw new BusinessException(EventHubErrorCodes.MentorAlreadyExist)
                    .WithData("Name", input.Name);
            }
            var info = new CreateMentorDto
            {
                Email = input.Email,
                Name = input.Name,
                DateOfBirth = input.DateOfBirth,
                PhoneNumber = input.PhoneNumber
            };

            var mentorAvatar = MentorAvatar;
            if (mentorAvatar != null && mentorAvatar.Length > 0)
            {
                await _qBoxFileAppService.SaveBlobAsync(new SaveBlobInputDto
                {
                    Name = string.Join("/", MentorAvatarRoot, CurrentUser.GetId()) + avatarExtension,
                    File = mentorAvatar
                });
            }

            var mentor = new Mentor(CurrentUser.GetId(), info.Email, info.Name, info.DateOfBirth, info.PhoneNumber, string.Join("/", MentorAvatarRoot, CurrentUser.GetId()) + avatarExtension);
            await _mentorRepository.InsertAsync(mentor,true);

            // Update Permission
            //await _permissionManager.SetForUserAsync(CurrentUser.GetId(), "QBox.Bookings.Create", false);
            //await _permissionManager.SetForUserAsync(CurrentUser.GetId(), "QBox.Slots.Update", true);
            //await _permissionManager.SetForUserAsync(CurrentUser.GetId(), "QBox.Slots.Delete", true);
            //await _permissionManager.SetForUserAsync(CurrentUser.GetId(), "QBox.Slots.Create", true);
            //await _permissionManager.SetForUserAsync(CurrentUser.GetId(), "QBox.Bookings.Accept", true);
            //await _permissionManager.SetForUserAsync(CurrentUser.GetId(), "QBox.Bookings.Deny", true);

            return ObjectMapper.Map<Mentor, MentorDto>(mentor);
        }

        private async Task<bool> IsMentorExisted()
        {
            return CurrentUser.Id.HasValue && await _mentorRepository
                .AnyAsync(x => x.Id == CurrentUser.GetId());
        }

        [Authorize]
        public async Task<PagedResultDto<MentorInListDto>>GetMentorsBySubjectAsync(MentorListFilterDto input)
        {
            var totalCount = await _mentorRepository.GetCountAsync(
                input.MajorSubstring,
                input.SubjectSubstring);

            var items = await _mentorRepository.GetListAsync(
                input.Sorting,
                input.SkipCount,
                MentorConsts.MaxMentorsInList,
                input.MajorSubstring,
                input.SubjectSubstring);

            var mentors = ObjectMapper.Map<List<MentorWithDetails>, List<MentorInListDto>>(items);

            return new PagedResultDto<MentorInListDto>(totalCount, mentors);
        }

        private byte[] TransferIntArrayToByteArray(int[] array)
        {
            byte[] result = new byte[array is null ? 0 : array.Length];
            for(int i = 0; i < result.Length; i++)
            {
                result[i] = byte.Parse(array[i] + "");
            }
            return result;
        }

        [Authorize]
        public async Task<List<SlotInListDto>> GetSlotsAsync(SlotListFilterDto input)
        {
            var items = await _slotRepository.GetListAsync(
                input.mentorId,
                input.minStartTime,
                input.maxStartTime);

            var slots = ObjectMapper.Map<List<SlotWithDetails>, List<SlotInListDto>>(items);
            return slots;
        }

        private void CheckIfValidOwnerAsync(Slot @slot)
        {
            if(@slot.MentorId != CurrentUser.GetId())
            {
                throw new AbpAuthorizationException(EventHubErrorCodes.NotAuthorizedToUpdateSlot);
            }
        }

        [Authorize(QBoxPermissions.Slots.Create)]
        public async Task AddMentorSkillAsync(AddMentorSkillDto input)
        {
            var @mentor = await _mentorRepository.GetAsync(CurrentUser.GetId(), true);
            
            @mentor.AddMentorSkill(GuidGenerator.Create(), input.SubjectId, input.Title, input.Description);


            await _mentorRepository.UpdateAsync(@mentor, true);
        }

        [Authorize(QBoxPermissions.Slots.Create)]
        public async Task AddSlotAsync(AddSlotDto input)
        {
            var @mentor = await _mentorRepository.GetAsync(CurrentUser.GetId(), true);
            @mentor.AddSlot(GuidGenerator.Create(), input.StartTime, input.EndTime);

            await _mentorRepository.UpdateAsync(@mentor, true);
        }

        [Authorize(QBoxPermissions.Slots.Update)]
        public async Task UpdateSlotAsync(Guid id, UpdateSlotDto input)
        {
            var @mentor = await _mentorRepository.GetAsync(CurrentUser.GetId(), true);
            @mentor.UpdateSlot(id, input.StartTime, input.EndTime, input.Status);

            await _mentorRepository.UpdateAsync(@mentor, true);
        }

        [Authorize(QBoxPermissions.Slots.Delete)]
        public async Task DeleteSlotAsync(Guid id)
        {
            var @slot = await _slotRepository.GetAsync(id, true);
            CheckIfValidOwnerAsync(@slot);

            var @mentor = await _mentorRepository.GetAsync(CurrentUser.GetId(), true);
            @mentor.RemoveSlot(id);

            await _mentorRepository.UpdateAsync(@mentor);
        }

        [Authorize]
        public async Task<PagedResultDto<BookingInListDto>> GetBookingsAsync(BookingListFilterDto input)
        {
            var items = await _bookingRepository.GetListAsync(
                input.Sorting,
                input.SkipCount,
                BookingConsts.MaxBookingResultCount,
                input.mentorId,
                input.minStartTime);

            var totalCount = await _bookingRepository.GetCountAsync(
                input.mentorId,
                input.minStartTime);
            

            var bookings = ObjectMapper.Map<List<BookingWithDetails>, List<BookingInListDto>>(items);
            return new PagedResultDto<BookingInListDto>(totalCount, bookings);
        }

        [Authorize(QBoxPermissions.Bookings.Accept)]
        public async Task AcceptBookingAsync(Guid slotId, Guid bookingId)
        {
            var @mentor = await _mentorRepository.GetAsync(CurrentUser.GetId(), true);
            var @slot = await _slotRepository.GetAsync(slotId, true);
            CheckIfValidOwnerAsync(@slot);
            @slot.UpdateBooking(bookingId, (byte) BookingStatus.Accepted);

            await _slotRepository.UpdateAsync(@slot);
        }

        [Authorize(QBoxPermissions.Bookings.Deny)]
        public async Task DenyBookingAsync(Guid slotId, Guid bookingId)
        {
            var @mentor = await _mentorRepository.GetAsync(CurrentUser.GetId(), true);
            var @slot = await _slotRepository.GetAsync(slotId, true);
            CheckIfValidOwnerAsync(@slot);
            @slot.UpdateBooking(bookingId, (byte)BookingStatus.Denied);

            await _slotRepository.UpdateAsync(@slot);
        }
    }
}