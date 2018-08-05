using SWN.MobileService.Api.Data.Entities;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public interface IGetWordBackService
    {
        Task<int> UpdateResponse(MessageDetail message, int getWordBackOptionId);
    }
}