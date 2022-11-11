using EventHub.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EventHub.Admin.Permissions
{
    public class EventHubPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //QBox
            var qBoxGroup = context.AddGroup(QBoxPermissions.QBoxGroupName);

            qBoxGroup.AddPermission(QBoxPermissions.Subjects.Default, L("Permission:SubjectManagement"));

            qBoxGroup.AddPermission(QBoxPermissions.Slots.Default, L("Permission:SlotManagement"));
            qBoxGroup.AddPermission(QBoxPermissions.Slots.Create, L("Permission:SlotCreate"));
            qBoxGroup.AddPermission(QBoxPermissions.Slots.Update, L("Permission:SlotEdit"));
            qBoxGroup.AddPermission(QBoxPermissions.Slots.Delete, L("Permission:SlotDelete"));

            qBoxGroup.AddPermission(QBoxPermissions.Bookings.Default, L("Permission:BookingManagement"));
            qBoxGroup.AddPermission(QBoxPermissions.Bookings.Create, L("Permission:BookingCreate"));
            qBoxGroup.AddPermission(QBoxPermissions.Bookings.Accept, L("Permission:BookingAccept"));
            qBoxGroup.AddPermission(QBoxPermissions.Bookings.Deny, L("Permission:BookingDeny"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<EventHubResource>(name);
        }
    }
}
