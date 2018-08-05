using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWN.MobileService.Api.Models;
using SWN.MobileService.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Controllers
{
    [Route("api/getwordback")]
    [Authorize]
    public class GetWordBackController : ControllerBase
    {
        public readonly IMessageService _messageService;
        public readonly IGetWordBackService _getWordBackService;

        public GetWordBackController(IMessageService messageService, IGetWordBackService getWordBackService)
        {
            _messageService = messageService;
            _getWordBackService = getWordBackService;
        }

        /// <summary>
        /// Text Message GWB Response update
        /// </summary>
        /// <remarks>
        /// ### Usage Notes ###
        /// 1. A valid message Id must be provided as a querystring
        /// </remarks>
        /// <response code="202">OK - If response accepted.</response>
        /// <response code="404">If message not found.</response>
        /// <response code="104">Invalid username or email address.</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="getWordBack"></param>
        /// <returns>
        /// List of contacts with account details
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GetWordBackRequest getWordBack)
        {
            var message = await _messageService.GetMessage(getWordBack.MessageId);

            if (message == null)
            {
                return NotFound();
            }

            UserInfo userInfo = new UserInfo(User);

            if (message.MobileRecipients.Any(x => x.MobileUserId != userInfo.MobileUserId))
                return Forbid();

            await _getWordBackService.UpdateResponse(message, getWordBack.GetWordBackOptionId);

            return Accepted();
        }
    }
}