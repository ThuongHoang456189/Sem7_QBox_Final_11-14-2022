namespace EventHub.Organizations.Mentors
{
    public static class MentorConsts
    {
        public const string DefaultSorting = "Name asc";

        public const int MinAge = 16;
        public const int MaxEmailLength = 128;
        public const int MinNameLength = 1;
        public const int MaxNameLength = 128;
        public const int MinPhoneNumberLength = 10;
        public const int MaxPhoneNumberLength = 12;
        public const int MaxAvatarLength = 256;
        public const string DefaultAvatar = "avatars/default-mentor.jpg";

        public static string[] AllowedAvatarImageExtensions = { ".jpg", ".png" };

        public const int MaxMentorsInList = 30;
    }
}
