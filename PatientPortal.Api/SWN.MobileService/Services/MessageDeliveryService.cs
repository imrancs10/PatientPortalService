using Microsoft.Extensions.Configuration;
using SWN.MobileService.Api.Data.Entities;
using SWN.MobileService.Api.Infrastructure.Adaptors;
using SWN.MobileService.Api.PushNotification.Contracts;
using SWN.MobileService.Api.PushNotification.Messages;
using SWN.MobileService.Api.PushNotification.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public class MessageDeliveryService : IMessageDeliveryService
    {
        private readonly IConfiguration _configuration;
        private readonly IMobileApiClient _mobileApiClient;
        private readonly IPushNotificationService _pushNotificationService;
        const string GetFCMTokenUrl = "/api/fcmtokens?";
        const string NotificationTitle = "OnSolve Notification";
        const string NotificationBody = "There is a new notification from OnSolve";

        public MessageDeliveryService(IConfiguration configuration,
            IMobileApiClient mobileApiClient, IPushNotificationService pushNotificationService)
        {
            _configuration = configuration;
            _mobileApiClient = mobileApiClient;
            _pushNotificationService = pushNotificationService;
        }

        public async Task DeliverMessage(MessageDetail messageDetail)
        {
            List<int> mobileUserIds = messageDetail.MobileRecipients.Select(x => (int)x.MobileUserId).ToList();
            var requestURI = CreateGetFCMTokensUri(mobileUserIds);
            IList<string> fcmTokens = await _mobileApiClient.GetFCMTokens(requestURI);

            if (fcmTokens.Count > 0)
            {
                var requestMessage = new RequestMessage
                {
                    Body =
                {
                    RegistrationIds = fcmTokens,
                    Notification = new NotificationModel
                    {
                        Title = NotificationTitle,
                        Body = NotificationBody
                    },
                    Data = new Dictionary<string, string>
                    {
                        { "MessageId", messageDetail.Id.ToString() },
                        { "MessageType", "Text" }
                    }
                }
                };

                 await _pushNotificationService.PushMessage(requestMessage);
            }
        }

        private string CreateGetFCMTokensUri(List<int> mobileUserIds)
        {
            StringBuilder RequestUri = new StringBuilder(GetFCMTokenUrl);
            var p = mobileUserIds.Select(x => string.Format("{0}={1}", WebUtility.UrlEncode("mobileUserIds"), WebUtility.UrlEncode(x.ToString())));
            return RequestUri.Append(string.Join("&", p)).ToString();
        }
    }
}
