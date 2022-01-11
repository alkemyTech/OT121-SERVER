using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs.ActivitiesDTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
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
    public class ActivitiesTestUpdate : BaseTest
    {
        #region Object 
        private ApplicationDbContext _context;
        private AuthController authController;
        private IConfiguration _configuration;

        private IActivitiesServices _activitiesServices;
        private ActivitiesController _activitiesController;

        private IUserServices _userService;
        private IMailService _mailService;
        private IImageService _imageServices;
        private IOptions<JWTSettings> _jWTSettings;

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

            _activitiesServices = new ActivitiesServices(unitOfWork, _imageServices);
            _activitiesController = new ActivitiesController(_activitiesServices);

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

        #region Test method UpdateById_Return_Ok
        [TestMethod]
        public async Task Update_Should_Return_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 1;

            ActivitiesUpdateDTO activitiesUpdateDTO = new ActivitiesUpdateDTO()
            {
                Id = 1,
                Name = "Update activity",
                Content = "Update content",
                Image = null
            };

            //Act
            var response = await _activitiesController.UpdatePutAsync(id, activitiesUpdateDTO);
            var expected = response as OkObjectResult;
            // Assert
            Assert.AreEqual(200, expected.StatusCode);
        }
        #endregion

        #region Test method Test method UpdateById_Return_BadRequest
        [TestMethod]
        public async Task Update_Should_Return_BadRequest()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 2;

            ActivitiesUpdateDTO activitiesUpdateDTO = new ActivitiesUpdateDTO()
            {
                Id = 1,
                Name = "Update activity",
                Content = "Update content",
                Image = GetImage()
            };

            //Act
            var response = await _activitiesController.UpdatePutAsync(id, activitiesUpdateDTO);
            var expected = response as BadRequestObjectResult;
            // Assert
            Assert.AreEqual(400, expected.StatusCode);
        }
        #endregion

        #region Test method UpdateById_Return_BadRequest_NotFound_Id
        [TestMethod]
        public async Task Update_Should_Return_BadRequest_NotFound_Id()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 2;

            ActivitiesUpdateDTO activitiesUpdateDTO = new ActivitiesUpdateDTO()
            {
                Id = 2,
                Name = "Update activity",
                Content = "Update content",
                Image = GetImage()
            };

            //Act
            var response = await _activitiesController.UpdatePutAsync(id, activitiesUpdateDTO);
            var expected = response as BadRequestObjectResult;
            // Assert
            Assert.AreEqual(400, expected.StatusCode);
        }
        #endregion

        #region Test method Update_Unauthorized_User
        [TestMethod]
        public async Task Update_Unauthorized_User()
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

        #region Test method UpdateById_Return_Name_Field_Is_Required
        [TestMethod]
        public void Update_Should_Return_Name_Field_Is_Required()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedMessage = "The Content field is required.";

            ActivitiesUpdateDTO activitiesUpdateDTO = new ActivitiesUpdateDTO()
            {
                Id = 1,
                Name = "Update activity",
                Image = GetImage()
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(activitiesUpdateDTO);

            //Act

            Validator.TryValidateObject(activitiesUpdateDTO, validationContext, result);
            // Assert
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        #endregion

        #region Test method UpdateById_Return_Content_Field_Is_Required
        [TestMethod]
        public void Update_Should_Return_Content_Field_Is_Required()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            var expectedMessage = "The Name field is required.";

            ActivitiesUpdateDTO activitiesUpdateDTO = new ActivitiesUpdateDTO()
            {
                Id = 1,
                Content = "Update content",
                Image = GetImage()
            };
            var result = new List<ValidationResult>();
            var validationContext = new ValidationContext(activitiesUpdateDTO);

            //Act

            Validator.TryValidateObject(activitiesUpdateDTO, validationContext, result);
            // Assert
            Assert.AreEqual(expectedMessage, result.FirstOrDefault().ErrorMessage);
        }
        #endregion

        #region Private method UserRol
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

        #region Private method SeedContacts
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

            var activities = new Activities
            {
                Name = "Organization ",
                Content = "Content",
                Image = null
            };

            context.Add(role);
            context.Add(roleStandard);

            context.Add(user);
            context.Add(userLogin);

            context.Add(activities);
            context.SaveChanges();
        }
        #endregion
    }
}