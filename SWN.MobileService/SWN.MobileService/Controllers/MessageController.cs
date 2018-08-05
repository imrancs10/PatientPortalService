using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWN.MobileService.Api.Enums;
using SWN.MobileService.Api.Models;
using SWN.MobileService.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController : ControllerBase
    {
        public readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Get Express Text Message based on message id
        /// </summary>
        /// <remarks>
        /// ### Usage Notes ###
        /// 1. A valid message Id must be provided
        /// </remarks>
        /// <response code="200">OK - If response created.</response>
        /// <response code="400">Message is not a text message.</response>
        /// <response code="404">If message not found.</response>
        /// <response code="403">Message Id belong to another User.</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="messageId">message to be supplied</param>
        /// <returns>
        /// message detail belong to supplied message id
        /// </returns>
        [HttpGet]
        [Route("messageId")]
        public async Task<IActionResult> Get(long messageId)
        {
            var message = await _messageService.GetMessage(messageId);

            if (message == null)
            {
                return NotFound();
            }
            if (message.MessageType != MessageType.Text)
            {
                return BadRequest();
            }

            UserInfo userInfo = new UserInfo(User);
            if (message.MobileRecipients.Any(x => x.MobileUserId != userInfo.MobileUserId))
            {
                return Forbid();
            }

            TextMessageModel model = new TextMessageModel()
            {
                MessageText = message.MessageText,
                ExpirationDate = message.ExpirationDate,
                MessageTransactionId = message.MessageTransactionId,
                MessageId = message.Id
            };
            _messageService.SendMessageStatus(model);
            return Ok(model);
        }


        /// <summary>
        /// Get all the Message based on last seen message id
        /// </summary>
        /// <remarks>
        /// ### Usage Notes ###
        /// 1. A valid message Id must be provided.
        /// </remarks>
        /// <response code="200">OK - if success.</response>
        /// <response code="404">if messages not found.</response>
        /// <response code="500">Internal Server Error</response>
        /// <param name="messageId">message to be supplied</param>
        /// <returns>
        /// List of message belong to supplied message id
        /// </returns>
        [HttpGet]
        [Route("lastseen/{messageId}")]
        public async Task<IActionResult> Sync(long messageId)
        {
            UserInfo userInfo = new UserInfo(User);

            var result = await _messageService.GetMessageInfoList(messageId, userInfo.MobileUserId);
            if (result.Count == 0)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}