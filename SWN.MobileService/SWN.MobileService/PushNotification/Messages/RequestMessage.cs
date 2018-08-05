using Newtonsoft.Json;
using SWN.MobileService.Api.PushNotification.Serialization;

namespace SWN.MobileService.Api.PushNotification.Messages
{
    public class RequestMessage
    {
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RequestMessage()
        {
            Header = new RequestMessageHeader();
            Body = new RequestMessageBody();

            _jsonSerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new PropertyNameResolver()
            };
        }
        public RequestMessageHeader Header { get; set; }

        public RequestMessageBody Body { get; set; }
        public string BodyToJSON
        {
            get { return JsonConvert.SerializeObject(Body, Formatting.Indented, _jsonSerializerSettings); }
        }
        public Error? ValidateRequest()
        {
            if (Body.RegistrationIds == null)
                return Error.MissingRegistration;

            return null;
        }
    }
}
