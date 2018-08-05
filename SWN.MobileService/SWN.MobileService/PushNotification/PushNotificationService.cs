using Newtonsoft.Json;
using SWN.MobileService.Api.PushNotification.Contracts;
using SWN.MobileService.Api.PushNotification.Messages;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.PushNotification
{
    public class PushNotificationService : IPushNotificationService
    {
        private const string FirebaseEndpoint = "https://fcm.googleapis.com/fcm/send";
        private readonly string _firebaseServerKey;

        public PushNotificationService(string firebaseServerKey)
        {
            _firebaseServerKey = firebaseServerKey;
        }

        public async Task<ResponseMessage> PushMessage(RequestMessage requestMessage)
        {
            var requestValidationStatus = requestMessage.ValidateRequest();
            if (requestValidationStatus != null)
            {
                throw new ResponseMessageErrorException(((Error)requestValidationStatus).ToString());
            }

            ResponseMessage cloudResponse = await SendRequestMessage(requestMessage);

            return cloudResponse;
        }

        private async Task<ResponseMessage> SendRequestMessage(RequestMessage fcmRequestMessage)
        {
            var jsonBody = fcmRequestMessage.BodyToJSON;
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, FirebaseEndpoint)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, fcmRequestMessage.Header.ContentType)
            };

            httpRequest.Headers.TryAddWithoutValidation("Authorization", "key=" + _firebaseServerKey);

            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
                var responsePayload = await httpResponse.Content.ReadAsStringAsync();
                ResponseMessageBody cloudResponseBody = JsonConvert.DeserializeObject<ResponseMessageBody>(responsePayload);

                ResponseMessage cloudResponse = new ResponseMessage();
                cloudResponse.Header = new ResponseMessageHeader() { ResponseStatusCode = httpResponse.StatusCode };
                cloudResponse.Body = cloudResponseBody;

                return cloudResponse;
            }
        }

    }
}
