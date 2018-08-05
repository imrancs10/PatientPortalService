namespace SWN.MobileService.Api.PushNotification.Messages
{
    public enum Error
    {
        MissingRegistration,
        InvalidRegistration,
        NotRegistered,
        InvalidPackageName,
        Unauthorized,
        MismatchSenderId,
        BadRequest,
        InvalidParameters,
        MessageTooBig,
        InvalidDataKey,
        InvalidTtl,
        Unavailable,
        InternalServerError,
        DeviceMessageRateExceeded,
        TopicsMessageRateExceeded,
        InvalidApnsCredential
    }
}
