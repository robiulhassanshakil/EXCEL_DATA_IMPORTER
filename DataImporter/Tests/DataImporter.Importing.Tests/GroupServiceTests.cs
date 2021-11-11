using System;
using System.Diagnostics.CodeAnalysis;
using Autofac.Extras.Moq;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Entities;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.Repositories;
using DataImporter.Importing.Services;
using DataImporter.Importing.UniteOfWorks;
using Moq;
using NUnit.Framework;
using Shouldly;
using Group = DataImporter.Importing.BusinessObjects.Group;

namespace DataImporter.Importing.Tests
{
    [ExcludeFromCodeCoverage]
    public class GroupServiceTests
    {
        private AutoMock _mock;
        private Mock<IImportingUnitOfWork> _importingUniteOfWorkMock;
        private Mock<IGroupRepository> _groupRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IGroupService _groupService;

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
            _importingUniteOfWorkMock = _mock.Mock<IImportingUnitOfWork>();
            _groupRepositoryMock = _mock.Mock<IGroupRepository>();
            _mapperMock = _mock.Mock<IMapper>();
            _groupService = _mock.Create<GroupService>();
        }

        [TearDown]
        public void TestCleanup()
        {
            _importingUniteOfWorkMock.Reset();
            _groupRepositoryMock.Reset();
            _mapperMock.Reset();
        }
        [Test]
        public void CreateGroup_GroupDoesNotExist_ThrowException()
        {
            //Arrange
            Group group = null;
            //Act and Assert
            Should.Throw<InvalidParameterException>(
                () => _groupService.CreateGroup(group)
            );
        }

        [Test]
        public void CreateGroup_GroupExist_CreateGroup()
        {
            //Arrange
            var group =new Group()
            {
                Name = "DevSkill"
            } ;
            var gp = new Entities.Group()
            {
                Name = group.Name
            };
            
            _importingUniteOfWorkMock.Setup(x => x.Groups).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Object.Add(gp);
            _importingUniteOfWorkMock.Setup(x => x.Save()).Verifiable();
            //Act
            _groupService.CreateGroup(group);
            //Assert
            this.ShouldSatisfyAllConditions(
                () => _importingUniteOfWorkMock.VerifyAll(),
                ()=>_groupRepositoryMock.VerifyAll());
        }
        [Test]
        public void DeleteGroup_GroupIdDoesNotExist_ThrowException()
        {
            var id = 0;
            Should.Throw<InvalidParameterException>(
                () => _groupService.DeleteGroup(id)
            );
        }
        [Test]
        public void DeleteGroup_GroupIdExist_DeleteGroup()
        {
            //Arrange
            var id = 1;
            _importingUniteOfWorkMock.Setup(x => x.Groups).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Object.Remove(id);
            _importingUniteOfWorkMock.Setup(x => x.Save()).Verifiable();

            //act

            _groupService.DeleteGroup(id);

            //Assert
            this.ShouldSatisfyAllConditions(
                    () => _importingUniteOfWorkMock.VerifyAll(),
                    () => _groupRepositoryMock.VerifyAll());
        }

        [Test]
        public void UploadGroup_GroupDoesNotExist_ThrowException()
        {
            //Arrange
            Group group = null;

            //Act and Assert
            Should.Throw<InvalidParameterException>(
                () => _groupService.UpdateGroup(group)
            );
        }
        [Test]
        public void UploadGroup_GroupExist_UploadGroup()
        {
            //Arrange
            var group = new Group()
            {
                Id = 1,
                Name = "C#"
            };
            var group2 = new Entities.Group()
            {
                Id = 1,
                Name = "asp.net"
            };
            _importingUniteOfWorkMock.Setup(x => x.Groups).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Setup(x=>x.GetById(group.Id)).Returns(group2).Verifiable(); 
            


            _importingUniteOfWorkMock.Setup(x => x.Save()).Verifiable();

            //act

            _groupService.UpdateGroup(group);

            //Assert
            this.ShouldSatisfyAllConditions(
                () => _importingUniteOfWorkMock.VerifyAll(),
                () => _groupRepositoryMock.VerifyAll());
        }
        [Test]
        public void UploadGroup_DatabaseGroupDoesNotExist_ThrowException()
        {
            //Arrange
            var group = new Group()
            {
                Id = 1,
                Name = "C#"
            };
            Entities.Group group2 = null;
            _importingUniteOfWorkMock.Setup(x => x.Groups).Returns(_groupRepositoryMock.Object);
            _groupRepositoryMock.Setup(x => x.GetById(group.Id)).Returns(group2).Verifiable();

            //act

            //Assert
            Should.Throw<InvalidParameterException>(
                () => _groupService.UpdateGroup(group)); 
        }
    }
}