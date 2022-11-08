using AutoMapper;
using EventHub.Countries;
using EventHub.Events;
using AutoMapper;
using EventHub.Countries;
using EventHub.Events;
using EventHub.Events.Registrations;
using EventHub.Knowledges.Categories;
using EventHub.Members;
using EventHub.Organizations;
using EventHub.Organizations.Memberships;
using EventHub.Organizations.Mentees.Bookings;
using EventHub.Organizations.Mentors;
using EventHub.Organizations.Mentors.Slots;
using EventHub.Organizations.Plans;
using EventHub.Users;
using Volo.Abp.AutoMapper;
using Volo.Abp.Identity;

namespace EventHub
{
    public class EventHubApplicationAutoMapperProfile : Profile
    {
        public EventHubApplicationAutoMapperProfile()
        {
            CreateMap<Organization, OrganizationInListDto>();
            CreateMap<Organization, OrganizationProfileDto>();
            CreateMap<Organization, OrganizationDto>();

            CreateMap<IdentityUser, OrganizationMemberDto>();

            CreateMap<Event, EventDto>();
            CreateMap<Event, EventInListDto>()
                .Ignore(x => x.OrganizationName)
                .Ignore(x => x.OrganizationDisplayName)
                .Ignore(x => x.IsLiveNow);
            CreateMap<Event, EventDetailDto>()
                .Ignore(x => x.OrganizationId)
                .Ignore(x => x.OrganizationName)
                .Ignore(x => x.OrganizationDisplayName);

            CreateMap<Track, TrackDto>();

            CreateMap<Session, SessionDto>();
            
            CreateMap<Speaker, SpeakerDto>()
                .Ignore(x => x.UserName);

            CreateMap<IdentityUser, EventAttendeeDto>();

            CreateMap<Event, EventLocationDto>()
                .Ignore(x => x.Country);
            
            CreateMap<Country, CountryLookupDto>();

            CreateMap<IdentityUser, UserDto>();
            
            CreateMap<UserWithoutDetails, UserInListDto>();

            CreateMap<PlanInfoDefinition, PlanInfoDefinitionDto>();
            CreateMap<FeatureOfPlanDefinition, FeatureOfPlanDefinitionDto>();

            // QBox
            CreateMap<SubjectWithDetails, SubjectLookupDto>()
                .ForMember(dest => dest.MajorTitle, opts => opts.MapFrom(src => src.Major.Title));

            CreateMap<Mentor, MentorDto>();
            CreateMap<MentorWithDetails, MentorInListDto>();
            CreateMap<SlotWithDetails, SlotInListDto>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => ((byte)src.Status)));
            CreateMap<BookingWithDetails, BookingInListDto>();
        }
    }
}
