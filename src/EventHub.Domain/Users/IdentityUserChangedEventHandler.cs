using EventHub.Organizations.Mentees;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Identity;

namespace EventHub.Users
{
    public class IdentityUserChangedEventHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
    {
        private readonly IRepository<Mentee, Guid> _menteeRepository;

        public IdentityUserChangedEventHandler(
            IRepository<Mentee, Guid> menteeRepository)
        {
            _menteeRepository = menteeRepository;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            var mentee = new Mentee(eventData.Entity.Id, eventData.Entity.Email, string.IsNullOrWhiteSpace(eventData.Entity.Name) ? eventData.Entity.Email : eventData.Entity.Name, null, eventData.Entity.PhoneNumber, MenteeConsts.DefaultAvatar);
            await _menteeRepository.InsertAsync(mentee);
        }
    }
}
