using BulletinBoard.DB.Models;
using BulletinBoard.Interfaces;
using BulletinBoard.Web;
using BulletinBoard.Web.CustomModels;
using BulletinBoard.Web.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BulletinBoard.Tests
{
    public class Tests
    {
        [Test]
        public void GetAllBulletins_DefaultPageParams_OrderedByNameAsync()
        {
            var data = BulletinTestMethods.Setup();
            var pageParams = new PageParams();
            var orderedDogs = BulletinTestMethods.GetOrderedListByAttribute(data, pageParams);

            Mock<IRepository<Bulletin>> BulletinRepo = new();
            BulletinRepo.Setup(c => c.Get()).Returns(data.AsQueryable());
            var controller = new BulletinsService(BulletinRepo.Object);


            var result = controller.GetAllBulletins(pageParams, true).Result;
            bool isEqual = BulletinTestMethods.ObjectsComparer(orderedDogs, result);


            Assert.AreEqual(orderedDogs.Count, result.Count);
            Assert.True(isEqual);
        }

        [Test]
        public void GetAllBulletins_SortingDesc_IsEqualCollection()
        {
            var data = BulletinTestMethods.Setup();
            var pageParams = new PageParams() { Order = "desc" };
            var orderedDogs = BulletinTestMethods.GetOrderedListByAttribute(data, pageParams);

            Mock<IRepository<Bulletin>> BulletinsRepo = new();
            BulletinsRepo.Setup(c => c.Get()).Returns(data.AsQueryable());
            var controller = new BulletinsService(BulletinsRepo.Object);

            var result = controller.GetAllBulletins(pageParams, true).Result;
            bool isEqual = BulletinTestMethods.ObjectsComparer(orderedDogs, result);

            Assert.AreEqual(orderedDogs.Count, result.Count);
            Assert.True(isEqual);
        }

        [Test]
        public void GetAllBulletins_FilledWrongParams_IsEmptyCollections()
        {
            var data = BulletinTestMethods.Setup();
            var pageParams = new PageParams() { Order = "ASCENING", Attribute = "Nome", PageNumber = 100500, PageSize = 20000 };
            var orderedDogs = BulletinTestMethods.GetOrderedListByAttribute(data, pageParams);

            Mock<IRepository<Bulletin>> DogRepo = new();
            DogRepo.Setup(c => c.Get()).Returns(data.AsQueryable());
            var controller = new BulletinsService(DogRepo.Object);

            var result = controller.GetAllBulletins(pageParams, true).Result;
            bool isEqual = BulletinTestMethods.ObjectsComparer(orderedDogs, result);

            Assert.AreEqual(orderedDogs.Count, result.Count);
            Assert.True(isEqual);
        }

        [Test]
        public async Task CreateBulletin_CorrectInputParams_BulletinCreatedAsync()
        {
            var newBulletin = new Bulletin 
            {
                Name = "Продам Тест!", 
                CreationDate = DateTime.Now, 
                Description = "Test Test Test", 
                ID = Guid.NewGuid(), 
                PhotoLinks = "test1.jpg", 
                Price = 50
            };

            Mock<IRepository<Bulletin>> BulletinRepo = new();
            BulletinRepo.Setup(c => c.CreateAsync(It.IsAny<Bulletin>()).Result).Returns(true);
            var controller = new BulletinsService(BulletinRepo.Object);

            var result = await controller.CreateBulletin(newBulletin);

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(Bulletin), result);
        }

        [Test]
        public async Task CreateBulletin_WrongPrice_NotCreated()
        {
            var newBulletin = new Bulletin
            {
                Name = "Продам Плохой Тест!",
                CreationDate = DateTime.Now,
                Description = "BadTest BadTest BadTest",
                ID = Guid.NewGuid(),
                PhotoLinks = "test1.jpg",
                Price = -100
            };

            Mock<IRepository<Bulletin>> BulletinRepo = new();
            BulletinRepo.Setup(c => c.CreateAsync(It.IsAny<Bulletin>()).Result).Returns(false);
            var controller = new BulletinsService(BulletinRepo.Object);

            var result = await controller.CreateBulletin(newBulletin);

            Assert.Null(result);
        }
    }
}