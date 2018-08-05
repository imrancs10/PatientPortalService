using AsyncPoco;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SWN.MobileService.Api.Data;
using SWN.MobileService.Api.Data.Entities;
using SWN.MobileService.Api.Services;
using System;
using System.Threading.Tasks;

namespace SWN.MobileService.Test.Services
{
    [TestClass]
    public class GetWordBackServiceTest
    {
        private Mock<IDatabase> _swn402db;
        [TestInitialize]
        public void Arrange()
        {
            _swn402db = new Mock<IDatabase>();
        }

        [TestMethod]
        public async Task UpdateGetWordBackResponse_TestAsync()
        {
            _swn402db.Setup(x => x.ExecuteAsync(It.IsAny<Sql>()))
                   .Returns(Task.FromResult(1));
            MessageDetail messageDetail = new MessageDetail
            {
                MessageTransactionId = 1,
                RecipientId = 1,
                ContactPointId = 1
            };

            GetWordBackService service = new GetWordBackService(_swn402db.Object);
            var result = await service.UpdateResponse(messageDetail, 1);
            result.Should().Be(1);
        }

    }
}
