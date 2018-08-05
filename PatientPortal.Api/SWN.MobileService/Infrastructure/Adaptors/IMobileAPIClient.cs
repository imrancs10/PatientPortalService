using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Infrastructure.Adaptors
{
    public interface IMobileApiClient
    {
        Task<List<int>> GetMobileUsers(string requestUri);
        Task<List<string>> GetFCMTokens(string requestUri);
    }
}