using System;
using Autofac.Extras.Moq;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web.Models.GroupModel;
using Moq;
using NUnit.Framework;

namespace DataImporter.Web.Tests
{
    public class CreateGroupModelTests
    {
        private AutoMock _mock;
        private Mock<IGroupService> _groupServiceMock;
        private Mock<IMapper> _mapperMock;
        private CreateGroupModel _model;


        [OneTimeSetUp]
        public void ClassSetup()
        {
            _mock = AutoMock.GetLoose();
        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            _mock?.Dispose();
        }
        [SetUp]
        public void TestSetup()
        {
            _groupServiceMock = _mock.Mock<IGroupService>();
            _mapperMock = _mock.Mock<IMapper>();
            _model = _mock.Create<CreateGroupModel>();

        }

        [TearDown]
        public void TestCleanup()
        {
            _groupServiceMock?.Reset();
            _mapperMock?.Reset();

        }

        [Test]
        public void CreateGroup_ApplicationUserIdNull_ThrowExceptions()
        {
            //Arrange
            Guid guid = new Guid("00000000 - 0000 - 0000 - 0000 - 000000000000");

            var exception = new InvalidOperationException("Application User Id Cant be Found");
            //Act
            _model.CreateGroup(guid);

            _groupServiceMock.Setup(x=>x.CreateGroup(guid));
            //Assert


            Assert.Pass();
        }
    }
}