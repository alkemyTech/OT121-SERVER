using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs.NewsDTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Test.Helper;

namespace Test.UnitTest.ActivitiesControllerTest
{
    [TestClass]
    public class NewsTestCreate : BaseTest
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
        private IOptions<JWTSettings> _jWTSettings;

        private IImageService _imageServices;
        private IOptions<AWSSettings> _aWSSettings;
        private S3AwsHelper _s3AwsHelper;
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

            _aWSSettings = Options.Create(new AWSSettings()
            {
                Bucket = ""
            });

            _s3AwsHelper = new S3AwsHelper(_aWSSettings);

            _imageServices = new ImageService(_aWSSettings);
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

        #region Created_News_Should_Return_Ok
        [TestMethod]
        public async Task Created_News_Should_Return_Created()
        {
            //Arrange
            Cleanup();
            MakeArrange();

            NewsCreateDTO newsCreateDTO = new NewsCreateDTO()
            {
                Name = "News",
                Content = "Content",
                Image = GetImage()
            };

            //Act
            var response = await _newsController.CreateAsync(newsCreateDTO);
            var expected = response as ObjectResult;
            // Assert
            Assert.AreEqual(201, expected.StatusCode);
        }
        #endregion

        #region Created_News_Should_Return_Bad_Request_Image_Is_Required
        [TestMethod]
        public void Created_News_Should_Return_Bad_Request_Image_Is_Required()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedMessage = "The Image field is required.";

            NewsCreateDTO newsCreateDTO = new NewsCreateDTO()
            {
                Name = "News",
                Content = "Content",
                Image = null
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(newsCreateDTO);

            //Act

            Validator.TryValidateObject(newsCreateDTO, validationContext, result);
            // Assert
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        #endregion

        #region Created_News_Should_Return_Bad_Request_Name_Is_Required
        [TestMethod]
        public void Created_News_Should_Return_Bad_Request_Name_Is_Required()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedMessage = "The Name field is required.";

            NewsCreateDTO newsCreateDTO = new NewsCreateDTO()
            {
                Content = "Content",
                Image = GetImage()
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(newsCreateDTO);

            //Act

            Validator.TryValidateObject(newsCreateDTO, validationContext, result);
            // Assert
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        #endregion


        #region Created_News_Should_Return_Bad_Request_Content_Is_Required
        [TestMethod]
        public void Created_News_Should_Return_Bad_Request_Content_Is_Required()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedMessage = "The Content field is required.";

            NewsUpdateDTO newsUpdateDTO = new NewsUpdateDTO()
            {
                Id = 1,
                Name = "News",
                Content = null,
                Image = GetImage()
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(newsUpdateDTO);

            //Act

            Validator.TryValidateObject(newsUpdateDTO, validationContext, result);
            // Assert
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        #endregion

        #region Created_User_Unauthorized
        [TestMethod]
        public async Task Created_User_Unauthorized()
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

        #region Private method GetImage
        private IFormFile GetImage()
        {
            byte[] bytes = Encoding.UTF8.GetBytes("fake content");

            var file = new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "fileUpload",
                fileName: "image.jpg"
                )
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
                ContentDisposition = "form-data; name=\"fileUpload\"; filename=\"image.jpg\""
            };
            return file;
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