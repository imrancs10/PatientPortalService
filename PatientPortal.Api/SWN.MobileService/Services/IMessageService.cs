using SWN.MobileService.Api.Data.Entities;
using SWN.MobileService.Api.Enums;
using SWN.MobileService.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public interface IMessageService
    {
        Task<MessageDetail> GetMessage(long messageId);
        bool SendMessageStatus(TextMessageModel model);
        Task<MessageProcessState> ProcessMessage(ExpressItem expressItem);
        Task<IList<MessageInfoModel>> GetMessageInfoList(long messageId, int mobileUserId);
    }
}