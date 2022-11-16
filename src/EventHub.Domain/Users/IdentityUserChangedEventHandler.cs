using EventHub.Organizations.Mentees;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace EventHub.Users
{
    public class IdentityUserChangedEventHandler : ILocalEventHandler<EntityCreatedEventData<IdentityUser>>, ITransientDependency
    {
        private readonly IRepository<Mentee, Guid> _menteeRepository;
        private readonly IPermissionManager _permissionManager;
        protected IPermissionGrantRepository _permissionGrantRepository { get; }
        protected IGuidGenerator _guidGenerator { get; }

        public IdentityUserChangedEventHandler(
            IRepository<Mentee, Guid> menteeRepository,
            IPermissionManager permissionManager,
            IPermissionGrantRepository permissionGrantRepository,
            IGuidGenerator guidGenerator)
        {
            _menteeRepository = menteeRepository;
            _permissionManager = permissionManager;
            _permissionGrantRepository = permissionGrantRepository;
            _guidGenerator = guidGenerator;
        }

        public async Task HandleEventAsync(EntityCreatedEventData<IdentityUser> eventData)
        {
            var mentee = new Mentee(eventData.Entity.Id, eventData.Entity.Email, string.IsNullOrWhiteSpace(eventData.Entity.Name) ? eventData.Entity.Email : eventData.Entity.Name, null, eventData.Entity.PhoneNumber, MenteeConsts.DefaultAvatar);
            await _menteeRepository.InsertAsync(mentee);

            //await _permissionGrantRepository.InsertAsync(
            //    new PermissionGrant(
            //        _guidGenerator.Create(), "QBox.Subjects",
            //        "U", eventData.Entity.Id.ToString()));
            //await _permissionGrantRepository.InsertAsync(
            //    new PermissionGrant(
            //        _guidGenerator.Create(), "QBox.Slots",
            //        "U", eventData.Entity.Id.ToString()));
            //await _permissionGrantRepository.InsertAsync(
            //    new PermissionGrant(
            //        _guidGenerator.Create(), "QBox.Bookings",
            //        "U", eventData.Entity.Id.ToString()));
            //await _permissionGrantRepository.InsertAsync(
            //    new PermissionGrant(
            //        _guidGenerator.Create(), "QBox.Bookings.Create",
            //        "U", eventData.Entity.Id.ToString()));
            //await _permissionManager.SetAsync("QBox.Slots", "U", eventData.Entity.Id.ToString().ToLower(), true);
            //await _permissionManager.SetAsync("QBox.Bookings", "U", eventData.Entity.Id.ToString().ToLower(), true);
            //await _permissionManager.SetAsync("QBox.Bookings.Create", "U", eventData.Entity.Id.ToString().ToLower(), true);
            //await _permissionManager.SetForUserAsync(eventData.Entity.Id, "QBox.Subjects", true);
            //await _permissionManager.SetForUserAsync(eventData.Entity.Id, "QBox.Slots", true);
            //await _permissionManager.SetForUserAsync(eventData.Entity.Id, "QBox.Bookings", true);
            //await _permissionManager.SetForUserAsync(eventData.Entity.Id, "QBox.Bookings.Create", true);
            //await _permissionManager.SetForUserAsync(eventData.Entity.Id, "AbpIdentity.Roles", true);
        }
    }
}
