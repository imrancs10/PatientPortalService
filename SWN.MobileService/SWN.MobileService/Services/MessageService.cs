using AsyncPoco;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SWN.MobileService.Api.Data;
using SWN.MobileService.Api.Data.Entities;
using SWN.MobileService.Api.Enums;
using SWN.MobileService.Api.Infrastructure.Adaptors;
using SWN.MobileService.Api.Models;
using SWN.Notification.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWN.MobileService.Api.Services
{
    public class MessageService : IMessageService
    {
        private readonly Func<MobileServiceContext> _dbProvider;
        private readonly IConfiguration _configuration;
        const string Baseurl = "OnSolveMobileServiceUri";
        private const string mediaType = "application/json";
        private const string GetUsersUrl = "/api/user?";
        private readonly IDatabase _swn402Db;
        private IMessageDeliveryService _messageDeliveryService;
        private const string SWNDIRECTEXPIREHOURS = "SWNDIRECT_EXPIRE_HOURS";
        const string feedbackDetail = "express text message";
        readonly IMessageQueueService _messageQueueService;
        readonly IMobileApiClient _mobileApiClient;

        public MessageService(IDatabase swn402Db
            , Func<MobileServiceContext> dbProvider
            , IConfiguration configuration
            , IMobileApiClient mobileApiClient
            , IMessageQueueService messageQueueService
            , IMessageDeliveryService messageDeliveryService)
        {
            _swn402Db = swn402Db;
            _dbProvider = dbProvider;
            _configuration = configuration;
            _mobileApiClient = mobileApiClient;
            _messageQueueService = messageQueueService;
            _messageDeliveryService = messageDeliveryService;

        }

        public async Task<MessageProcessState> ProcessMessage(ExpressItem expressItem)
        {
            var messageState = MessageProcessState.MessageFetchedFromQueue;
            var messages = await SaveMessageInDB(expressItem);
            messageState = MessageProcessState.MessageSavedInDB;

            foreach (var msg in messages)
            {
                if (msg.ExpirationDate > DateTime.UtcNow)
                {
                   await _messageDeliveryService.DeliverMessage(msg);
                }

            }
            messageState = MessageProcessState.MessageBroadcastedToUsers;
            return messageState;
        }


        public async Task<MessageDetail> GetMessage(long messageId)
        {
            using (var dbContext = _dbProvider())
            {
                return await dbContext.MessageDetails.Include(recipient => recipient.MobileRecipients)
                    .FirstOrDefaultAsync(msg => msg.Id == messageId);
            }
        }

        public bool SendMessageStatus(TextMessageModel model)
        {
            var status = MessageTransactionStatusCode.ExpressMessageStatusReceived;
            var eventType = MessageTransactionEvent.EventClass.EMAIL_STATUS_EXPRESS;
            var feedback = new MessageTransactionFeedback(model.MessageTransactionId,
                                                          model.MessageId, status,
                                                          eventType, feedbackDetail,
                                                          DateTime.Now);
            return _messageQueueService.SendMessage(feedback, feedback.Id.ToString());
        }
        public async Task<IList<MessageInfoModel>> GetMessageInfoList(long lastSeenMessageId, int mobileUserId)
        {
            using (var dbContext = _dbProvider())
            {
                return await dbContext.MessageDetails.Include(user => user.MobileRecipients)
                    .Where(msg => msg.MobileRecipients.Any(x => x.MobileUserId == mobileUserId) && msg.Id > lastSeenMessageId)
                    .Select(md => new MessageInfoModel() { MessageId = md.Id, MessageType = md.MessageType })
                    .ToListAsync();
            }
        }


        private async Task<List<MessageDetail>> SaveMessageInDB(ExpressItem expressItem)
        {
            var expireInterval = await GetMessageExpirationInterval(expressItem.Message.Accountid);
            List<MessageDetail> messages = new List<MessageDetail>();
            using (var dbContext = _dbProvider())
            {
                foreach (var recipient in expressItem.Recipients)
                {
                    var isTransanctionValid = long.TryParse(recipient.id, out var transactionId);
                    if (isTransanctionValid)
                    {
                        var messageTransaction = await GetMessageTransactionDetails(transactionId);
                        string requestUri = CreateGetUsersUri(messageTransaction.RecipientId);
                        var users = await _mobileApiClient.GetMobileUsers(requestUri);

                        var mobileRecipients = new List<MobileRecipient>();

                        users.ForEach(user => mobileRecipients.Add(new MobileRecipient
                        {
                            MobileUserId = user

                        }));

                        var message = new MessageDetail
                        {
                            MessageText = expressItem.Message.Text,
                            ExpirationDate = DateTime.UtcNow.Add(expireInterval),
                            MobileRecipients = mobileRecipients,
                            MessageTransactionId = transactionId,
                            RecipientId = messageTransaction.RecipientId,
                            ContactPointId = messageTransaction.ContactPointId,
                            MessageType = Enums.MessageType.Text
                        };
                        messages.Add(message);
                    }
                }
                dbContext.MessageDetails.AddRange(messages);
                await dbContext.SaveChangesAsync();
                return messages;

            }
        }

        private async Task<TimeSpan> GetMessageExpirationInterval(string accountId)
        {
            var fetchContactsQuery = Sql.Builder.Select(" num_value ")
                                                .From("cfg_Acct_Settings cfg")
                                                .InnerJoin("member m")
                                                .On("cfg.member_id = m.member_id")
                                                .Append("WHERE key_value ='" + SWNDIRECTEXPIREHOURS + "' ")
                                                .Append(" And account_id = @0", accountId);


            var result = await _swn402Db.FirstOrDefaultAsync<int>(fetchContactsQuery);
            if (result <= 0)
            {
                result = Convert.ToInt32(_configuration[SWNDIRECTEXPIREHOURS]);
            }
            return TimeSpan.FromHours(result);
        }

        private async Task<MessageTransactionDetails> GetMessageTransactionDetails(long transactionId)
        {
            var fetchContactsQuery = Sql.Builder.Select(" date_time_sent,recipient_id, message_isGetWordBack ")
                                                  .From("msg_TransactionDetails")
                                                  .Append("WHERE msg_tran_id = @0", transactionId);
            return await _swn402Db.FirstOrDefaultAsync<MessageTransactionDetails>(fetchContactsQuery);
        }

        private string CreateGetUsersUri(long recipientId)
        {
            return GetUsersUrl + "recipientId=" + recipientId;
        }

    }
}
