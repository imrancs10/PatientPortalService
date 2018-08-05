using SWN.Notification.BusinessEntities;

namespace SWN.MobileService.Api.Services
{
    public interface IMessageQueueService
    {
        bool SendMessage(MessageTransactionFeedback message, string label);
    }
}
