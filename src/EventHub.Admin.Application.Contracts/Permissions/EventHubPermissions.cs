﻿namespace EventHub.Admin.Permissions
{
    public static class EventHubPermissions
    {
        public const string GroupName = "EventHub";

        public static class Organizations
        {
            public const string Default = GroupName + ".Organizations";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
            
            public static class Memberships
            {
                public const string Default = Organizations.Default + ".Memberships";
            }
        }

        public static class Events
        {
            public const string Default = GroupName + ".Events";
            public const string Update = Default + ".Update";

            public class Registrations
            {
                public const string Default = Events.Default + ".Registrations";
                public const string AddAttendee = Default + ".AddAttendee";
                public const string RemoveAttendee = Default + ".RemoveAttendee";
            }
        }

        public static class Users
        {
            public const string Default = GroupName + ".Users";
        }

        // QBox
        public const string QBoxGroupName = "QBox";

        public static class Subjects
        {
            public const string Default = QBoxGroupName + ".Subjects";
        }

        public static class Slots
        {
            public const string Default = QBoxGroupName + ".Slots";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Bookings
        {
            public const string Default = QBoxGroupName + ".Bookings";
            public const string Create = Default + ".Create";
            public const string Accept = Default + ".Accept";
            public const string Deny = Default + ".Deny";
        }
    }
}