using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.UnitTest
{
    [TestClass]
    public class TestContactsController
    {
        public Mock<IContactsServices> _contactsServices = new();
        public Mock<IMailService> _mailService = new();
        public ContactDTO contactDTO = new ContactDTO()
        {
            Name = "dario",
            Email = "dframirez@gmail.com",
            Message = "Probando",
            Phone = 12345
        };

        #region Test method Get
        [TestMethod]
        public async Task Get_Should_Return_Action_Ok()
        {
            // arrange
            int id = 1;
           
            _contactsServices.Setup(a => a.EntityExists(id)).Returns(true);
            _contactsServices.Setup(b => b.GetById(id)).ReturnsAsync(contactDTO);
            var expected = StatusCodes.Status200OK;

            var controller = new OngProject.Controllers.ContactsController(_contactsServices.Object, _mailService.Object);
            // act
            var response = await controller.Get(id);
            // assert
            var resp = (ObjectResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }

        [TestMethod]
        public async Task Get_Should_Return_Action_NotFound()
        {
            // arrange
            int id = 1;

            _contactsServices.Setup(a => a.EntityExists(id)).Returns(false);
            _contactsServices.Setup(b => b.GetById(id)).ReturnsAsync(contactDTO);
            var expected = StatusCodes.Status404NotFound;

            var controller = new OngProject.Controllers.ContactsController(_contactsServices.Object, _mailService.Object);
            // act
            var response = await controller.Get(id);
            // assert
            var resp = (StatusCodeResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }

        [TestMethod]
        public async Task Get_Should_Return_Action_BadRequest()
        {
            // arrange
            int id = 1;

            _contactsServices.Setup(a => a.EntityExists(id)).Returns(true);
            object p = _contactsServices.Setup(b => b.GetById(id)).Throws(new Exception("my exception"));
            var expected = StatusCodes.Status400BadRequest;

            var controller = new OngProject.Controllers.ContactsController(_contactsServices.Object, _mailService.Object);
            // act
            var response = await controller.Get(id);
            // assert
            var resp = (ObjectResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }
        #endregion

       
    }
}
