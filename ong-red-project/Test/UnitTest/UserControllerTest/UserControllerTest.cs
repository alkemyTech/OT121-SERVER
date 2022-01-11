using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test.UnitTest.UserTests
{
    [TestClass]
    public class UserControllerTest : CommonTest
    {
        #region Private
        private IUserServices _userServices;
        private UsersController _usersController;
        #endregion

        public override void AddControllers()
        {
            _usersController = new UsersController(_userServices);
        }

        public override void AddSeeds(ApplicationDbContext context)
        {

        }

        public override void AddServices()
        {
            _userServices = new UserServices(unitOfWork,_configuration, _imageServices, _jWTSettings);
        }
 
        [TestMethod]
        public async Task GetAllAsyncUsers_StatusOk()
        {
                Cleanup();
                MakeArrange();
                var response = (await _usersController.GetAsync()) as OkObjectResult;
                Assert.IsNotNull(response.Value);
                Assert.AreEqual(StatusCodes.Status200OK, response.StatusCode);
        }

        [TestMethod]
        public async Task PatchAsync_StatusOk()
        {
            Cleanup();
            MakeArrange();
            
            User user = new User()
            {
                Id = 1,
                FirstName = "Jose",
                LastName = "Rodriguez",
                Email = "joserodriguez@hotmail.com"
            };

            JsonPatchDocument jpD = new JsonPatchDocument();
            Type tUser = user.GetType();

            foreach (var prop in tUser.GetProperties())
            {
                var newValue = prop.GetValue(user, null);
                if(newValue != null){
                    jpD.Replace(prop.Name,newValue);
                }
            }
            
            var response = (await _usersController.PatchAsync(user.Id,jpD));

            var objectResult = response as OkObjectResult;
            Assert.IsNotNull(objectResult.Value);
            Assert.IsFalse((objectResult.Value as Result).HasErrors);
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [TestMethod]
        public async Task PatchAsync_StatusBadRequest()
        {
            Cleanup();
            MakeArrange();

            User user = new User()
            {
                Id = 2,
                FirstName = "Jose",
                LastName = "Rodriguez",
                Email = "email_3@example.com"
            };

            JsonPatchDocument jpD = new JsonPatchDocument();
            Type tUser = user.GetType();

            foreach (var prop in tUser.GetProperties())
            {
                var newValue = prop.GetValue(user, null);
                if (newValue != null)
                {
                    jpD.Replace(prop.Name, newValue);
                }
            }

            var response = (await _usersController.PatchAsync(user.Id, jpD));

            var objectResult = response as OkObjectResult;
            Assert.IsNotNull(objectResult.Value);
            Assert.IsFalse((objectResult.Value as Result).Messages[0].EndsWith("exitosamente."),"PatchAsync message should be end with en uso when is failed.");
            Assert.IsTrue((objectResult.Value as Result).Messages[0].EndsWith("en uso."), "PatchAsync message should be end exitosamente when is ok.");
            Assert.IsTrue((objectResult.Value as Result).HasErrors, "PathAsync should be HasErrors when failed.");
            Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode,"PathAsync Should be ends with Status Code 400 when fail.");
        }
    }
}