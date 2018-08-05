using System;
using System.Linq;
using System.Security.Claims;

namespace SWN.MobileService.Api.Models
{
    public class UserInfo
    {
        public UserInfo(ClaimsPrincipal claimsPrincipal)
        {
            MobileUserId = Convert.ToInt32(claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "MobileUserId").Value);
            DeviceToken = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "DeviceToken").Value;
        }

        public int MobileUserId { get; }
        public string DeviceToken { get; }

    }
}
