using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Controllers;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;
namespace Test.UnitTest.ContactsControllerTest
{
    [TestClass]
    public class GetTest : CommonTest
    {
        #region Test method Get
        [TestMethod]
        public async Task Get_Should_Return_Action_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 1;
            var expected = StatusCodes.Status200OK;
            //Act
            var response = await contactsController.Get(id);

            // Assert
            var resp = (ObjectResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }
        [TestMethod]
        public async Task Get_Should_Return_Action_NotFound()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 10;
            var expected = StatusCodes.Status404NotFound;

            //Act
            var response = await contactsController.Get(id);

            // Assert
            var resp = (ObjectResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }

        [TestMethod]
        public async Task Get_Should_Return_Action_BadRequest()
        {
            // arrange
            int id = 1;
            var _contactsServices = new Mock<IContactsServices>();
            var _mailService = new Mock<IMailService>();
            _contactsServices.Setup(a => a.EntityExists(id)).Returns(true);
            object p = _contactsServices.Setup(b => b.GetById(id)).Throws(new Exception("my exception"));
            var expected = StatusCodes.Status400BadRequest;
            var controller = new ContactsController(_contactsServices.Object, _mailService.Object);

            // act
            var response = await controller.Get(id);

            // assert
            var resp = (ObjectResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }
        #endregion

    }
}