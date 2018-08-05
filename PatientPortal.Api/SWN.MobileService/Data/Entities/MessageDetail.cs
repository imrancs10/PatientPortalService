using SWN.MobileService.Api.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWN.MobileService.Api.Data.Entities
{
    [Serializable]
    public class MessageDetail
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string MessageText { get; set; }
        [Required]
        public long MessageTransactionId { get; set; }
        public long ContactPointId { get; set; }
        public long RecipientId { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        public IList<MobileRecipient> MobileRecipients { get; set; }
        public MessageType MessageType { get; set; }
    }
}
