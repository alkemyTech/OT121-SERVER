using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            var controller = new ContactsController(_contactsServices.Object, _mailService.Object);
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

            var controller = new ContactsController(_contactsServices.Object, _mailService.Object);
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
            _contactsServices.Setup(b => b.GetById(id)).Throws(new Exception("my exception"));
            var expected = StatusCodes.Status400BadRequest;

            var controller = new ContactsController(_contactsServices.Object, _mailService.Object);
            // act
            var response = await controller.Get(id);
            // assert
            var resp = (ObjectResult)response.Result;
            Assert.AreEqual(expected, resp.StatusCode);
        }
        #endregion

        #region Test method ContactsAsync
        [TestMethod]
        public async Task ContactsAsync_Should_Return_Action_Ok()
        {
            // arrange
            _contactsServices.Setup(a => a.RegisterAsync(contactDTO)).ReturnsAsync(contactDTO);
            var expected = StatusCodes.Status200OK;

            var controller = new ContactsController(_contactsServices.Object, _mailService.Object);
            // act
            var response = await controller.ContactsAsync(contactDTO);
            // assert

            var resp = (ObjectResult)response;
            Assert.AreEqual(expected, resp.StatusCode);
        }

        #endregion

        #region Test method GetAllAsync
        [TestMethod]
        public async Task GetAllAsync_Should_Return_Action_Ok()
        {
            // arrange
            var contactDtoList = new List<ContactDTO>() { contactDTO, contactDTO };
            _contactsServices.Setup(b => b.GetAll()).ReturnsAsync(contactDtoList);
            var expected = StatusCodes.Status200OK;

            var controller = new ContactsController(_contactsServices.Object, _mailService.Object);
            // act
            var response = await controller.GetAllAsync();
            // assert
            var resp = (ObjectResult)response;
            Assert.AreEqual(expected, resp.StatusCode);
        }
        #endregion

        #region Test Dto ContactDTO
        [TestMethod]
        public void ContactDTO_Should_Return_Name_Field_Is_Required()
        {
            // arrange
            var expectedMessage = "The Name field is required.";
            var expectedCount= 1;
            ContactDTO contactDTOFail = new ContactDTO()
            {
                Email = "dto@dto.com",
                Message = "Probando",
                Phone = 12345
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(contactDTOFail);

            // act
            Validator.TryValidateObject(contactDTOFail, validationContext, result);

            // assert
            Assert.AreEqual(expectedCount, result.Count());
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        [TestMethod]
        public void ContactDTO_Should_Return_Email_Field_Is_Required()
        {
            // arrange
            var expectedMessage = "The Email field is required.";
            var expectedCount = 1;
            ContactDTO contactDTOFail = new ContactDTO()
            {
                Name = "Test",
                Message = "Probando",
                Phone = 12345
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(contactDTOFail);

            // act
            Validator.TryValidateObject(contactDTOFail, validationContext, result);

            // assert
            Assert.AreEqual(expectedCount, result.Count());
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        [TestMethod]
        public void ContactDTO_Should_Return_Name_Field_Is_Required_And_Email_Field_Is_Required()
        {
            // arrange
            var expectedMessageName = "The Name field is required.";
            var expectedMessageEmail = "The Email field is required.";
            var expectedCount = 2;
            ContactDTO contactDTOFail = new ContactDTO()
            {
                Message = "Probando",
                Phone = 12345
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(contactDTOFail);

            // act
            Validator.TryValidateObject(contactDTOFail, validationContext, result);

            // assert
            Assert.AreEqual(expectedCount, result.Count());
            Assert.AreEqual(expectedMessageName, result.FirstOrDefault().ErrorMessage);
            Assert.AreEqual(expectedMessageEmail, result.LastOrDefault().ErrorMessage);
        }
        #endregion


    }
}
