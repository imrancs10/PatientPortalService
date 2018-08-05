using SWN.MobileService.Api.PushNotification.Messages;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.PushNotification.Contracts
{
    public interface IPushNotificationService
    {
        Task<ResponseMessage> PushMessage(RequestMessage requestMessage);
    }
}
