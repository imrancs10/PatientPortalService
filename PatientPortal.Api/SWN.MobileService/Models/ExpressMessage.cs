using System;

namespace SWN.MobileService.Api.Models
{
    [Serializable]
    public class ExpressMessage
    {
        public String MessageId;
        public String Source;
        public String Title;
        public String Text;
        public String Mode;
        public String Accountid;
        public String Attachments;
        public String LockBox;
        public String LockBoxAction;
        public String LockBoxRecipientFiletoken;
        public String AttachmentRecipientFiletoken;
        public int retryInterval;
        public int expirationPeriod;
        public int vanish;
        public DateTime MessageFailureTime;
        public DateTime LastRetryTime;
        public DateTime CurrentProcessTime;
        public String ClientsUniqueRecipientID;
        public Boolean IsRetry;
        public int expressmessagestatus;
        public int lockboxmessagestatus;
        public string emailid;
        public string[][] attachmentTokens;
        
        //Geo Contracts
        public string geoversion;
        public string geomethod;
        public string longitude;
        public string latitude;
        public string radiusinmeters;
        public string expirytime;
        public string geofenceaction;
        public string useroptin;
        public string uponreceipt;
        public string geooptinprompt;
        public string geoaltmessage;
    }
}
