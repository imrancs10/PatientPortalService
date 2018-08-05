using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SWN.MobileService.Test.Services
{
    public class StubHttpMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            return null;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
