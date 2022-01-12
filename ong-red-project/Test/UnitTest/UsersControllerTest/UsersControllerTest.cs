using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs.TestimonialsDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Helper;

namespace Test.UnitTest.UserTest
{
    [TestClass]
    public class UsersControllerTest
    {
        #region Objects and Constructor

        private IConfiguration _configuration;

        private IOptions<AWSSettings> _awsSettings;
        private IOptions<JWTSettings> _jwtSettings;

        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ITestimonialsServices _testimonialsServices;
        private IUserServices _usersServices;
        private UsersController _controllerTest;
        private IImageService _imageServices;
        private EntityMapper _mapper;

        private ApplicationDbContext MakeContext()
        {
            var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase<ApplicationDbContext>("TestDB").Options;
            var dbcontext = new ApplicationDbContext(opciones);
            return dbcontext;
        }

        [TestInitialize]
        public void BuildContext()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .AddUserSecrets<Secrets>()
                .Build();

                
            //Settings Secrets
                _awsSettings = Options.Create(_configuration.GetSection("AWSSettings").Get<AWSSettings>());
                _jwtSettings = Options.Create(_configuration.GetSection("JWTSettings").Get<JWTSettings>());
            ///

            _context = MakeContext();
            _unitOfWork = new UnitOfWork(_context);
            _imageServices = new ImageService(_awsSettings);
            _usersServices = new UserServices(_unitOfWork,_configuration,_imageServices,_jwtSettings);
            _controllerTest = new UsersController(_usersServices);
            SeedUsers(_context);
        }

        #endregion

        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        #region GetAllAsync Test
        [TestMethod]
        public async Task GetAllAsync_ShouldReturn_StatusOK()
        {
            //Arrange
            CleanUp();
            BuildContext();

            // Act
            var response = await _controllerTest.GetAsync();
            var result = response as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task Patch_UserId_StatusOk()
        {
            //Arrange
            CleanUp();
            BuildContext();

            JsonPatchDocument jpD = new JsonPatchDocument();
            jpD = jpD.Replace("email","newtestemail@gmail.com");

            //Act
            var response = await _controllerTest.PatchAsync(1, jpD);
            var result = response as OkObjectResult;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task Patch_UserId_StatusNotFound()
        {
            //Arrange
            CleanUp();
            BuildContext();

            int userId = 1;

            string expectedMessage = "El usuario no fue encontrado o el email ya esta en uso.";

            JsonPatchDocument jpD = new JsonPatchDocument();
            jpD = jpD.Replace("email", "email_5@example.com");

            //Act
            var response = await _controllerTest.PatchAsync(userId, jpD);
            var result = response as NotFoundObjectResult;
            var msgResponse = result.Value as Result;

            // Assert
            Assert.AreEqual(404, result.StatusCode);
            Assert.IsTrue(msgResponse.HasErrors);
            Assert.AreEqual(expectedMessage, msgResponse.Messages[0]);
        }

        #endregion Update

        private void SeedUsers(ApplicationDbContext context)
        {
            for (int i = 1; i <= 30; i++)
            {
                var user = new User
                {
                    FirstName = $"Name_{i}",
                    LastName = $"LastName_{i}",
                    Email = $"email_{i}@example.com"
                };

                context.Add(user);
            }
            context.SaveChanges();
        }
    }
}