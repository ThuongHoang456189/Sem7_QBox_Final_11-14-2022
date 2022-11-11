using EventHub.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EventHub.Admin.Permissions
{
    public class EventHubPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var eventHubGroup = context.AddGroup(EventHubPermissions.GroupName);

            var organizationsPermission = eventHubGroup.AddPermission(EventHubPermissions.Organizations.Default, L("Permission:OrganizationManagement"));
            organizationsPermission.AddChild(EventHubPermissions.Organizations.Create, L("Permission:Create"));
            organizationsPermission.AddChild(EventHubPermissions.Organizations.Update, L("Permission:Edit"));
            organizationsPermission.AddChild(EventHubPermissions.Organizations.Delete, L("Permission:Delete"));
            organizationsPermission.AddChild(EventHubPermissions.Organizations.Memberships.Default, L("Permission:MembershipManagement"));

            var eventPermissions = eventHubGroup.AddPermission(EventHubPermissions.Events.Default, L("Permission:EventManagement"));
            eventPermissions.AddChild(EventHubPermissions.Events.Update, L("Permission:Edit"));

            var eventRegistrationPermissions = eventHubGroup.AddPermission(EventHubPermissions.Events.Registrations.Default, L("Permission:RegistrationManagement"));
            eventRegistrationPermissions.AddChild(EventHubPermissions.Events.Registrations.AddAttendee, L("Permission:AddAttendee"));
            eventRegistrationPermissions.AddChild(EventHubPermissions.Events.Registrations.RemoveAttendee, L("Permission:RemoveAttendee"));

            var userPermissions = eventHubGroup.AddPermission(EventHubPermissions.Users.Default, L("Permission:UserManagement"));

            // QBox
            var qBoxGroup = context.AddGroup("QBox");

            qBoxGroup.AddPermission("Subject Management", L("Permission:SubjectManagement"));

            qBoxGroup.AddPermission("QBox.Slots", L("Permission:SlotManagement"));
            qBoxGroup.AddPermission("QBox.Slots.Create", L("Permission:SlotCreate"));
            qBoxGroup.AddPermission("QBox.Slots.Update", L("Permission:SlotEdit"));
            qBoxGroup.AddPermission("QBox.Slots.Delete", L("Permission:SlotDelete"));

            qBoxGroup.AddPermission("QBox.Bookings", L("Permission:BookingManagement"));
            qBoxGroup.AddPermission("QBox.Bookings.Create", L("Permission:BookingCreate"));
            qBoxGroup.AddPermission("QBox.Bookings.Accept", L("Permission:BookingAccept"));
            qBoxGroup.AddPermission("QBox.Bookings.Deny", L("Permission:BookingDeny"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EventHubResource>(name);
        }
    }
}
