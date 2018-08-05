using AsyncPoco;
using System;

namespace SWN.MobileService.Api.Models
{
    public class MessageTransactionDetails
    {
        [Column("date_time_sent")]
        public DateTime DateTimeSent { get; set; }

        [Column("recipient_id")]
        public long RecipientId { get; set; }

        [Column("message_isGetWordBack")]
        public bool IsGetWordBack { get; set; }

        [Column("contact_point_id")]
        public long ContactPointId { get; set; }
    }
}
