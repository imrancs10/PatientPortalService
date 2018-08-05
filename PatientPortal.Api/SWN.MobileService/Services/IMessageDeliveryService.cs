using SWN.MobileService.Api.Data.Entities;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public interface IMessageDeliveryService
    {
        Task DeliverMessage(MessageDetail messageDetail);
    }
}
