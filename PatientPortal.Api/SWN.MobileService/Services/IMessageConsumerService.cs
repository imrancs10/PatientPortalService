using System.Threading.Tasks;

namespace PatientPortalService.Api.Services
{
    public interface IMessageConsumerService
    {
        Task ConsumeMessages();
    }
}
