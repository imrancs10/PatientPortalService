using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using PatientPortalService.Api.Data;
using PatientPortalService.Api.Infrastructure.Utility;
using PatientPortalService.Api.Infrastructure.Adaptors;
using Microsoft.EntityFrameworkCore;

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
                    PatientPortalContext _db = new PatientPortalContext();
                    //get message to be sent from db 
                    var messages = _db.AppointmentInfo.Include(x => x.Patient).Include(x => x.Doctor).ThenInclude(x => x.Department)
                                                                .Where(x => DateTime.Now <= x.AppointmentDateFrom
                                                                    && x.AppointmentDateFrom.ToShortDateString() == DateTime.Now.ToShortDateString()
                                                                    && (x.Reminder == false || x.Reminder == null))
                                                                .ToList();
                    using (_logger.BeginScope("Process Message"))
                    {
                        foreach (var message in messages)
                        {
                            //process msg 
                            _logger.LogInformation($"Message Processed,for AppointmentId: {message.AppointmentId}");
                            Message msg = new Message()
                            {
                                MessageTo = message.Patient.Email,
                                MessageNameTo = message.Patient.FirstName + " " + message.Patient.MiddleName + (string.IsNullOrWhiteSpace(message.Patient.MiddleName) ? "" : " ") + message.Patient.LastName,
                                Subject = "Appointment Reminder",
                                Body = EmailHelper.GetAppointmentSuccessEmail(message.Patient.FirstName, message.Patient.MiddleName, message.Patient.LastName, message.Doctor.DoctorName, message.AppointmentDateFrom, message.Doctor.Department.DepartmentName)
                            };
                            try
                            {
                                //Send Notification
                                ISendMessageStrategy sendMessageStrategy = new SendMessageStrategyForEmail(msg);
                                sendMessageStrategy.SendMessages();

                                //Update Reminder Status
                                var _patientRow = _db.AppointmentInfo.Where(x => x.AppointmentId == message.AppointmentId).FirstOrDefault();
                                if (_patientRow != null)
                                {
                                    _patientRow.Reminder = true;
                                    _db.Entry(_patientRow).State = EntityState.Modified;
                                    _db.SaveChanges();
                                }

                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation("Message Sending fail for Appointment Id {0}.", message.AppointmentId);
                            }
                        }
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
