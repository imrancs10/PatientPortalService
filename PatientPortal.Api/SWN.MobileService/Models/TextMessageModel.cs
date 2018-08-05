using System;

namespace SWN.MobileService.Api.Models
{
    public class TextMessageModel
    {
        public string MessageText { get; set; }
        public DateTime ExpirationDate { get; set; }
        public long MessageTransactionId { get; set; }
        public long MessageId { get; set; }
    }
}
