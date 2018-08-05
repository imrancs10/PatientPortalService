using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SWN.Messaging.Msmq;
using SWN.MobileService.Api.Models;
using System;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public class MessageConsumerService : IMessageConsumerService
    {
        private ILogger<MessageConsumerService> _logger;
        private IMessageService _messageService;
        private IMessageQueue _queue;
        private readonly IConfiguration _configuration;
        private const int MessageWaitTimeoutInMinutes = 10;

        public MessageConsumerService(ILogger<MessageConsumerService> logger,
            IMessageService messageService,
            IMessageQueue queue,
            IConfiguration configuration)
        {
            _logger = logger;
            _messageService = messageService;
            _queue = queue;
            _configuration = configuration;
        }

        public async Task ConsumeMessages()
        {
            int.TryParse(_configuration["MessageWaitTimeoutInMinutes"], out int messageWaitTimeoutInMinutes);

            while (true)
            {
                try
                {
                    var message = await _queue.ReceiveAsync<ExpressItem>(TimeSpan.FromMinutes(messageWaitTimeoutInMinutes));
                    using (_logger.BeginScope("Within the second loop"))
                    {
                        await _messageService.ProcessMessage(message.Body);
                        _logger.LogInformation($"Message Processed, Id: {message.Body.Message.MessageId}");
                    }
                }
                catch (TimeoutException)
                {
                    _logger.LogInformation("Message not received within current timeframe.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message);
                    if (ex.InnerException != null)
                    {
                        _logger.LogCritical(ex, ex.Message);
                    }
                    throw;
                }
            }
        }
    }
}
