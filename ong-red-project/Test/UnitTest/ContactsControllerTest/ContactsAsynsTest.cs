using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Core.DTOs;
using System.Threading.Tasks;

namespace Test.UnitTest.ContactsControllerTest
{
    [TestClass]
    public class ContactsAsynsTest : CommonTest
    {
        [TestMethod]
        public async Task ContactsAsyns_Should_Return_Action_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var contactDto = new ContactDTO()
            {
                Name = "Contact valid",
                Phone = 381,
                Email = "test1@test.com",
                Message = "Message from contact "
            };
            var expected = StatusCodes.Status200OK;
            //Act
            var response = await contactsController.ContactsAsync(contactDto);

            // Assert
            var resp = (ObjectResult)response;
            Assert.AreEqual(expected, resp.StatusCode);
        }

        [TestMethod]
        public async Task ContactsAsyns_Should_Return_Data_Valid()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var ExpectedcontactDto = new ContactDTO()
            {
                Name = "Contact valid",
                Phone = 381,
                Email = "test1@test.com",
                Message = "Message from contact "
            };
            //Act
            var response = await contactsController.ContactsAsync(ExpectedcontactDto) as ObjectResult;

            // Assert
            var resp = response.Value as ContactDTO;
            Assert.AreEqual(ExpectedcontactDto.Name, resp.Name);
            Assert.AreEqual(ExpectedcontactDto.Phone, resp.Phone);
            Assert.AreEqual(ExpectedcontactDto.Email, resp.Email);
            Assert.AreEqual(ExpectedcontactDto.Message, resp.Message);
        }

    }
}
