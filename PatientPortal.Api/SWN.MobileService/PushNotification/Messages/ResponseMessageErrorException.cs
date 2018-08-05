using System;

namespace SWN.MobileService.Api.PushNotification.Messages
{
    public class ResponseMessageErrorException : Exception
    {
        public ResponseMessageErrorException(string message) : base(message) { }
    }
}
