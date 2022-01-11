using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Test.UnitTest.UserControllerTest
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
    }
}