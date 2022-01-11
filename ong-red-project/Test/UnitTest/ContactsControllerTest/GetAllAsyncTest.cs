using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Core.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.UnitTest.ContactsControllerTest
{
    [TestClass]
    public class GetAllAsyncTest : CommonTest
    {
        [TestMethod]
        public async Task GetAllAsync_Should_Return_Action_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expected = StatusCodes.Status200OK;
            //Act
            var response = await contactsController.GetAllAsync();

            // Assert
            var resp = (ObjectResult)response;
            Assert.AreEqual(expected, resp.StatusCode);
        }

        [TestMethod]
        public async Task GetAllAsync_Should_Return_Action_Ok_And_Count()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expected = StatusCodes.Status200OK;
            var expectedCount = 3;

            //Act
            var response = await contactsController.GetAllAsync();

            // Assert
            var resp1 = (ObjectResult)response;
            Assert.AreEqual(expected, resp1.StatusCode);
            var resp2 = resp1.Value as List<ContactDTO>;
            Assert.AreEqual(expectedCount, resp2.Count());
        }


    }
}