using System;

namespace SWN.MobileService.Api.Models
{ 
    [Serializable]
    public class ExpressItem
    {        
        public ExpressMessage Message { get; set; }
        public ExpressRecipient[] Recipients { get; set; }
    }
}
