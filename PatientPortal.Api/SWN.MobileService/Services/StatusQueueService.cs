using SWN.Messaging.Msmq;
using SWN.Notification.BusinessEntities;

namespace SWN.MobileService.Api.Services
{
    public class StatusQueueService : IMessageQueueService
    {
        private readonly IMessageQueue _queue;
        public StatusQueueService(IMessageQueue queue)
        {
            _queue = queue;
        }

        public bool SendMessage(MessageTransactionFeedback message, string label)
        {
            var queueMessage = _queue.CreateMessage(message, label, null);

            _queue.Send(queueMessage);

            return true;
        }
    }
}
