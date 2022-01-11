using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs.CommentsDTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Helper;

namespace Test.UnitTest.ActivitiesControllerTest
{
    [TestClass]
    public class NewsTestGetNewsAllComments : BaseTest
    {
        #region Objects
        private ApplicationDbContext _context;
        private AuthController authController;
        private IConfiguration _configuration;

        private INewsServices _newsServices;
        private NewsController _newsController;
        private IUriService _uriService;

        private IUserServices _userService;
        private IMailService _mailService;
        private IImageService _imageServices;
        private IOptions<JWTSettings> _jWTSettings;
        public object Request { get; private set; }
        #endregion

        #region TestInitialize
        [TestInitialize]
        public void MakeArrange()
        {
            var config = new Dictionary<string, string>{
                {"JWTSettings:Key", "1234567890qwertyuiop"},
                };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            _context = MakeContext("TestDb");

            _jWTSettings = Options.Create(new JWTSettings()
            {
                Key = "1234567890qwertyuiop",
                Issuer = "prueba",
                Audience = "prueba",
                DurationInDays = 1
            });
            IUnitOfWork unitOfWork = new UnitOfWork(_context);

            _newsServices = new NewsServices(unitOfWork, _imageServices, _uriService);
            _newsController = new NewsController(_newsServices);

            _userService = new UserServices(unitOfWork, _configuration, _imageServices, _jWTSettings);
            authController = new AuthController(_userService, _mailService);
            SeedContacts(_context);
        }
        #endregion

        #region TestCleanup
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
        #endregion

        #region Get_News_Should_Return_Ok
        [TestMethod]
        public async Task Get_News_Should_Return_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 1;
            int page = 1;
            //Act
            var response = await _newsController.GetAllCommentsByNews(id, page);
            var expected = response as ObjectResult;
            // Assert
            Assert.AreEqual(200, expected.StatusCode);
        }
        #endregion

        #region Get_News_Should_Return_Bad_Request
        [TestMethod]
        public async Task Get_News_Should_Return_Bad_Request()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 2;
            int page = 1;
            //Act
            var response = await _newsController.GetAllCommentsByNews(id, page);
            var expected = response as ObjectResult;
            // Assert
            Assert.AreEqual(400, expected.StatusCode);
        }
        #endregion

        #region Get_News_Count_Equals_Items
        [TestMethod]
        public async Task Get_News_Count_Equals_Items()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 1;
            int page = 1;
            //Act
            var response = await _newsController.GetAllCommentsByNews(id, page);
            var obj = response as ObjectResult;
            var expected = obj.Value as PaginationDTO<CommentResponseDTO>;
            // Assert
            Assert.AreEqual(1, expected.TotalItems);
        }
        #endregion

        #region Get_News_Count_Equals_Pages
        [TestMethod]
        public async Task Get_News_Count_Equals_Pages()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 1;
            int page = 1;
            //Act
            var response = await _newsController.GetAllCommentsByNews(id, page);
            var obj = response as ObjectResult;
            var expected = obj.Value as PaginationDTO<CommentResponseDTO>;
            // Assert
            Assert.AreEqual(1, expected.TotalPages);
        }
        #endregion

        #region Get_User_Unauthorized
        [TestMethod]
        public async Task Get_User_Unauthorized()
        {
            //Arrange
            Cleanup();
            MakeArrange();

            UserLoginRequestDTO userLogin = new UserLoginRequestDTO();
            userLogin.Email = "user@example.com";
            userLogin.Password = "12345";
            //Act
            var response = await authController.LoginAsync(userLogin) as ObjectResult;
            var user = response.Value as UserLoginResponseDTO;

            var result = UserRol(user);
            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region Private Method UserRol
        private bool UserRol(UserLoginResponseDTO user)
        {
            var claims = new[] { new Claim(ClaimTypes.Role, user.Role) };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var contextMock = new Mock<HttpContext>();
            contextMock.Setup(x => x.User).Returns(claimsPrincipal);

            authController.ControllerContext.HttpContext = contextMock.Object;

            return authController.User.IsInRole("Administrator");
        }

        #endregion

        #region Private Method SeedContacts
        private void SeedContacts(ApplicationDbContext context)
        {
            var role = new Role
            {
                Id = 1,
                Name = "Administrator",
                Description = "User Administrator"
            };

            var roleStandard = new Role
            {
                Id = 2,
                Name = "Standard",
                Description = "User Standard"
            };

            var user = new User
            {
                FirstName = "User",
                LastName = "Test",
                Email = "user@example.com",
                Password = Encrypt.GetSHA256("12345"),
                Photo = null,
                RoleId = 2
            };
            var userLogin = new User
            {
                FirstName = "UserLogin",
                LastName = "TestLogin",
                Email = "ailenadrianagomez@gmail.com",
                Password = Encrypt.GetSHA256("12345"),
                Photo = null,
                RoleId = 1
            };

            var category = new Category
            {
                Id = 1,
                Name = "News",
                Description = "Description",
                Image = null
            };

            var news = new News
            {
                Name = "News",
                Content = "Content",
                Image = null,
                CategoryId = 1
            };

            var comments = new Comments
            {
                UserId = 1,
                Body = "Body",
                NewId = 1
            };

            context.Add(role);
            context.Add(roleStandard);

            context.Add(user);
            context.Add(userLogin);
            context.Add(category);
            context.Add(news);
            context.Add(comments);
            
            context.SaveChanges();
        }

        #endregion
    }
}
