namespace SWN.MobileService.Api.Enums
{
    public enum MessageProcessState
    {
        MessageFetchedFromQueue = 0,
        MessageSavedInDB = 1,
        MessageBroadcastedToUsers = 2,
        MessageProcessingFailed
    }
}
