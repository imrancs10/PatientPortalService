using AsyncPoco;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SWN.MobileService.Api.Data;
using SWN.MobileService.Api.Data.Entities;
using SWN.MobileService.Api.Infrastructure.Adaptors;
using SWN.MobileService.Api.Services;
using SWN.Notification.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Messaging;
using System.Threading.Tasks;

namespace SWN.MobileService.Test.Services
{
    [TestClass]
    public class MessageServiceTest
    {
        Func<MobileServiceContext> _dbContextProvider;
        private IMessageService _messageService;
        Mock<IDatabase> _swn402Db;

        private readonly long mobileUserId = 1;
        private readonly long messageId = 1;
        private Mock<IMessageQueueService> _queueService;
        private Mock<IConfiguration> _configuration;
        Mock<IMobileApiClient> _mobileApiClient;
        private const string statusQueueConfig = "FormatName:DIRECT=OS:WIN2169\\Private$\\MESSAGE_STATUS_QUEUE";
        private Mock<IMessageDeliveryService> _messageDeliveryService;

        [TestInitialize]
        public void ArrangeElements()
        {
            _mobileApiClient = new Mock<IMobileApiClient>();
            _configuration = new Mock<IConfiguration>();
            _queueService = new Mock<IMessageQueueService>();
            _swn402Db = new Mock<IDatabase>();
            _messageDeliveryService = new Mock<IMessageDeliveryService>();
        }

        private MobileServiceContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<MobileServiceContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new MobileServiceContext(options);

            insertDataIntoMemoryDatabase(context, Api.Enums.MessageType.Text);

            context.SaveChanges();
            return context;
        }

        private MobileServiceContext GetContextWithDataForVoiceMessage()
        {
            var options = new DbContextOptionsBuilder<MobileServiceContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;
            var context = new MobileServiceContext(options);

            insertDataIntoMemoryDatabase(context, Api.Enums.MessageType.Voice);

            context.SaveChanges();
            return context;
        }

        private static void insertDataIntoMemoryDatabase(MobileServiceContext context, Api.Enums.MessageType messageType)
        {
            var recipient = new MobileRecipient { MobileUserId = 1 };

            var message = new MessageDetail
            {
                MessageType = messageType,
                ExpirationDate = DateTime.Now,
                MessageText = "This is test message",
                MessageTransactionId = 1,
                MobileRecipients = new List<MobileRecipient> { recipient }
            };
            context.MessageDetails.Add(message);
        }

        private MobileServiceContext GetContextWithEmptyData()
        {
            var options = new DbContextOptionsBuilder<MobileServiceContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new MobileServiceContext(options);

            dbContext.SaveChanges();

            return dbContext;
        }

        [TestMethod]
        public async Task GetExpressTextMessage_VerifyResponseObject_When_StatusQueue_SuccessfullySendMessage()
        {
            _dbContextProvider = GetContextWithData;
            _messageService = new MessageService(_swn402Db.Object, _dbContextProvider, _configuration.Object, _mobileApiClient.Object, _queueService.Object, _messageDeliveryService.Object);
            _queueService.Setup(x => x.SendMessage(It.IsAny<MessageTransactionFeedback>(), It.IsAny<string>())).Returns(true);
            var result = await _messageService.GetMessage(messageId);

            result.Should().NotBeNull();
        }

        [TestMethod]
        public async Task GetMessagesResponse_TestAsync()
        {
            _dbContextProvider = GetContextWithData;
            _messageService = new MessageService(_swn402Db.Object, _dbContextProvider, _configuration.Object, _mobileApiClient.Object, _queueService.Object, _messageDeliveryService.Object);

            var result = await _messageService.GetMessageInfoList(0, 1);
            result.Count.Should().Be(1);
        }
    }
}