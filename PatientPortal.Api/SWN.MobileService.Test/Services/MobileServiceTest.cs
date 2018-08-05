using AsyncPoco;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SWN.MobileService.Api.Data;
using SWN.MobileService.Api.Data.Entities;
using SWN.MobileService.Api.Infrastructure.Adaptors;
using SWN.MobileService.Api.Models;
using SWN.MobileService.Api.Enums;
using SWN.MobileService.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWN.MobileService.Test.Services
{
    [TestClass]
    public class MobileServiceTest
    {
        private Mock<IDatabase> _swn402Db;
        private Mock<IConfiguration> _configuration;
        private Mock<IMobileApiClient> _mobileApiClient;
        private Func<MobileServiceContext> _dbProvider;
        private Mock<IMessageQueueService> _queueService;
        private Mock<IMessageDeliveryService> _pushService;

        [TestInitialize]
        public void Arrange()
        {
            _swn402Db = new Mock<IDatabase>();
            _configuration = new Mock<IConfiguration>();
            _mobileApiClient = new Mock<IMobileApiClient>();
            _queueService = new Mock<IMessageQueueService>();
            //_httpClient = new Mock<HttpClient>();
            _pushService = new Mock<IMessageDeliveryService>();
        }

        [TestMethod]
        public async Task ExpireMessageTime_FromDB()
        {
            var msg = new MessageTransactionDetails
            {
                DateTimeSent = DateTime.Now,
                RecipientId = 5
            };

            _swn402Db.Setup(x => x.FirstOrDefaultAsync<int>(It.IsAny<Sql>()))
                   .Returns(Task.FromResult(72));

            _swn402Db.Setup(x => x.FirstOrDefaultAsync<MessageTransactionDetails>(It.IsAny<Sql>()))
                   .Returns(Task.FromResult(msg));

            _configuration.Setup(x => x["SWNDIRECT_EXPIRE_HOURS"]).Returns("42");
            _configuration.Setup(x => x["OnSolveMobileServiceUri"]).Returns("http://localhost:7012");
            _mobileApiClient.Setup(x => x.GetMobileUsers(It.IsAny<string>())).Returns(Task.FromResult(new List<int> { 1, 3 }));

            _dbProvider = GetContextWithData;
            var _messageService = new MessageService(_swn402Db.Object, _dbProvider
                , _configuration.Object, _mobileApiClient.Object, _queueService.Object, _pushService.Object);

            ExpressItem item = CreateExpressItem();

            var result = await _messageService.ProcessMessage(item);
            result.Should().Be(MessageProcessState.MessageBroadcastedToUsers);

        }

        private ExpressItem CreateExpressItem()
        {
            return new ExpressItem
            {
                Message = new ExpressMessage(),
                Recipients = new ExpressRecipient[] {
                    new ExpressRecipient
                    {
                        id = "10"
                    }, new ExpressRecipient
                    {
                        id = "5"
                    }}
            };
        }

        private MobileServiceContext GetContextWithData()
        {
            var options = new DbContextOptionsBuilder<MobileServiceContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            var dbContext = new MobileServiceContext(options);

            MessageDetail message = new MessageDetail
            {
                MessageText = "New Message",
                MobileRecipients = new List<MobileRecipient> {
                    new MobileRecipient
                    {
                        MobileUserId = 2
                    }},
                ExpirationDate = DateTime.UtcNow.Add(new TimeSpan(2, 0, 0))
            };

            dbContext.MessageDetails.Add(message);
            dbContext.SaveChanges();

            return dbContext;
        }

    }
}
