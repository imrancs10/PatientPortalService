using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Infrastructure.Adaptors
{
    public class MobileApiClient : IMobileApiClient
    {

        const string Baseurl = "OnSolveMobileServiceUri";
        private const string mediaType = "application/json";

        private readonly Func<HttpClient> _httpClientProvider;
        private readonly IConfiguration _configuration;

        public MobileApiClient(Func<HttpClient> httpClientProvider, IConfiguration configuration)
        {
            _httpClientProvider = httpClientProvider;
            _configuration = configuration;
        }


        public async Task<List<int>> GetMobileUsers(string requestUri)
        {
            List<int> users = new List<int>();
            var response = await SendRequest(requestUri);

            if (response.IsSuccessStatusCode)
            {
                var usersList = await response.Content.ReadAsStringAsync();
                users = JsonConvert.DeserializeObject<List<int>>(usersList);
            }

            return users;
        }

        public async Task<List<string>> GetFCMTokens(string requestURI)
        {
            List<string> fcmTokens = new List<string>();

            var response = await SendRequest(requestURI);

            if (response.IsSuccessStatusCode)
            {
                var usersList = await response.Content.ReadAsStringAsync();

                fcmTokens = JsonConvert.DeserializeObject<List<string>>(usersList);
            }

            return fcmTokens;
        }

        private async Task<HttpResponseMessage> SendRequest(string requestURI)
        {
            using (var client = _httpClientProvider())
            {
                client.BaseAddress = new Uri(_configuration[Baseurl]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                return await client.GetAsync(requestURI);
            }
        }
        
    }
}
