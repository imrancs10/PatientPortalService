using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SWN.Messaging.Msmq;
using System;
using System.Threading.Tasks;

namespace PatientPortalService.Api.Services
{
    public class MessageConsumerService : IMessageConsumerService
    {
        private ILogger<MessageConsumerService> _logger;
        private readonly IConfiguration _configuration;
        private const int MessageWaitTimeoutInMinutes = 10;

        public MessageConsumerService(ILogger<MessageConsumerService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task ConsumeMessages()
        {
            int.TryParse(_configuration["MessageWaitTimeoutInMinutes"], out int messageWaitTimeoutInMinutes);

            while (true)
            {
                try
                {

                    
                    //get message to be sent from db 
                    var message =  10;//await _queue.ReceiveAsync<ExpressItem>(TimeSpan.FromMinutes(messageWaitTimeoutInMinutes));
                    using (_logger.BeginScope("Within the second loop"))
                    {
                        //process msg 
                        //await _messageService.ProcessMessage(message.Body);
                        _logger.LogInformation($"Message Processed, Id: {10}"); //provide msg ID
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
