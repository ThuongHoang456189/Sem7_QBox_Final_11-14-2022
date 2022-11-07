namespace EventHub
{
    public static class EventHubErrorCodes
    {
        public const string OrganizationNameAlreadyExists = "EventHub:OrganizationNameAlreadyExists";
        public const string NotAuthorizedToCreateEventInThisOrganization = "EventHub:NotAuthorizedToCreateEventInThisOrganization";
        public const string EndTimeCantBeEarlierThanStartTime = "EventHub:EndTimeCantBeEarlierThanStartTime";
        public const string SessionEndTimeCantBeEarlierThanStartTime = "EventHub:SessionEndTimeCantBeEarlierThanStartTime";
        public const string CantRegisterOrUnregisterForAPastEvent = "EventHub:CantRegisterOrUnregisterForAPastEvent";
        public const string NotAuthorizedToUpdateOrganizationProfile = "EventHub:NotAuthorizedToUpdateOrganizationProfile";
        public const string CapacityOfEventFull = "EventHub:CapacityOfEventFull";
        public const string CapacityCanNotBeLowerThanRegisteredUserCount = "EventHub:CapacityCantBeLowerThanRegisteredUserCount";
        public const string NotAuthorizedToUpdateEvent = "EventHub:NotAuthorizedToUpdateEvent";
        public const string CantChangeEventTiming = "EventHub:CantChangeEventTiming";
        public const string SessionTimeShouldBeInTheEventTime = "EventHub:SessionTimeShouldBeInTheEventTime";
        public const string SessionTimeConflictsWithAnExistingSession = "EventHub:SessionTimeConflictsWithAnExistingSession";
        public const string UserNotFound = "EventHub:UserNotFound";
        public const string OrganizationNotFound = "EventHub:OrganizationNotFound";
        public const string TrackNameAlreadyExist = "EventHub:TrackNameAlreadyExist";
        public const string TrackNotFound = "EventHub:TrackNotFound";
        public const string SessionNotFound = "EventHub:SessionNotFound";
        public const string SessionTitleAlreadyExist = "EventHub:SessionTitleAlreadyExist";
        public const string CannotCreateNewEvent = "EventHub:CannotCreateNewEvent";
        public const string CannotAddNewTrack = "EventHub:CannotAddNewTrack";
        public const string CannotRegisterToEvent = "EventHub:CannotRegisterToEvent";

        // QBox
        public const string MajorTitleAlreadyExist = "QBox:MajorTitleAlreadyExist";
        public const string SubjectNotFound = "QBox:SubjectNotFound";
        public const string MentorAlreadyExist = "QBox:MentorAlreadyExist";
        public const string MentorSkillAlreadyExist = "QBox:MentorSkillAlreadyExist";
        public const string MentorSkillNotFound = "QBox:MentorSkillNotFound";
        public const string MentorShouldOlderThanMinAge = "QBox:MentorShouldOlderThanMinAge";
        public const string CertificateIssuanceDateShouldBeAfterMinAge = "QBox:CertificateIssuanceDateShouldBeAfterMinAge";
        public const string CertificateIssuanceDateShouldBeEarlierThanNow = "QBox:CertificateIssuanceDateShouldBeEarlierThanNow";
        public const string CertificateAlreadyExist = "QBox:CertificateAlreadyExist";
        public const string CertificateNotFound = "QBox:CertificateNotFound";
        public const string QuestionAlreadyExist = "QBox:QuestionAlreadyExist";
        public const string QuestionNotFound = "QBox:QuestionNotFound";
        public const string EndTimeCantBeEarlierThanStartTimePlusAmountOfTime = "QBox:EndTimeCantBeEarlierThanStartTimePlusAmountOfTime";
        public const string StartTimeCantBeEarlierThanNowPlusAmountOfTime = "QBox:StartTimeCantBeEarlierThanNowPlusAmountOfTime";
        public const string BookingRequestAlreadyCreated = "QBox:BookingRequestAlreadyCreated";
        public const string BookingNotFound = "QBox:BookingNotFound";
        public const string SlotIntervalsIntersect = "QBox:SlotIntervalsIntersect";
        public const string SlotNotFound = "Qbox:SlotNotFound";
        public const string SlotClosed = "QBox:SlotClosed";
        public const string SlotFullyBooked = "QBox:SlotFullyBooked";
        public const string NotAuthorizedToUpdateSlot = "QBox:NotAuthorizedToUpdateSlot";
    }
}
