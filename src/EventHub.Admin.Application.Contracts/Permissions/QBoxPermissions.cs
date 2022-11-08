using System;
using System.Collections.Generic;
using System.Text;

namespace EventHub.Admin.Permissions
{
    public static class QBoxPermissions
    {
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
