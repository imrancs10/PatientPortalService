using SWN.MobileService.Api.PushNotification.Model;
using System.Collections.Generic;

namespace SWN.MobileService.Api.PushNotification.Messages
{
    public class RequestMessageBody
    {
        public IDictionary<string, string> Data { get; set; }
        public IList<string> RegistrationIds { get; set; }
        public NotificationModel Notification { get; set; }
    }
}
