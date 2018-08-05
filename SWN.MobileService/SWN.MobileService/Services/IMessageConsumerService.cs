using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public interface IMessageConsumerService
    {
        Task ConsumeMessages();
    }
}
