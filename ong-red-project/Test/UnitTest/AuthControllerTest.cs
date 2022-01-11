using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Core.Services.AWS;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Configuration;
using Test.Helper;

namespace Test.UnitTest
{
    [TestClass]
    public class AuthControllerTest
    {
        #region Objects and Constructor

        private IConfiguration _configuration;

        private IOptions<MailSettings> _mailSettings;
        private IOptions<MailConstants> _mailConstants;
        private IOptions<JWTSettings> _jwtSettings;
        private IOptions<AWSSettings> _awsSettings;

        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private IMailService _mailService;
        private IImageService _imageService;
        private IUserServices _userServices;
        private AuthController _controllerTest;

        [TestInitialize]
        public void BuildContext()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .AddUserSecrets<Secrets>()
                .Build();
            //Settings Secrets
            _jwtSettings = Options.Create(_configuration.GetSection("JWTSettings").Get<JWTSettings>());

            _mailSettings = Options.Create(_configuration.GetSection("MailSettings").Get<MailSettings>());

            _mailConstants = Options.Create(_configuration.GetSection("MailConstants").Get<MailConstants>());

            _awsSettings = Options.Create(_configuration.GetSection("AWSSettings").Get<AWSSettings>());
            ///

            _context = ApplicationDbContextInMemory.GetContext();

            _unitOfWork = new UnitOfWork(_context);

            _mailService = new MailService(_mailSettings, _mailConstants);

            _imageService = new ImageService(_awsSettings);

            _userServices = new UserServices(_unitOfWork, _configuration, _imageService, _jwtSettings);

            _controllerTest = new AuthController(_userServices, _mailService);

            SeedUsers(_context);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        #endregion Objects and Constructor

        #region Login Tests

        [TestMethod]
        public async Task LoginAsync_WhenUserExists_ShouldReturn_StatusOKAndUserLoggedInfo()
        {
            // Arrange
            CleanUp();
            BuildContext();
            var userLogin = new UserLoginRequestDTO
            {
                Email = "daniel@email.com",
                Password = "1234"
            };

            // Act
            var response = await _controllerTest.LoginAsync(userLogin);
            var result = response as OkObjectResult;

            var userLoginInfo = result.Value as UserLoginResponseDTO;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(userLogin.Email, userLoginInfo.Email);
        }

        [TestMethod]
        public async Task LoginAsync_WhenUserDoesNotExists_ShouldReturn_BadRequestWhitExceptionMessage()
        {
            // Arrange
            CleanUp();
            BuildContext();
            var msg = "Usuario o contraseña incorrectos.";

            var userLogin = new UserLoginRequestDTO
            {
                Email = "jose@email.com",
                Password = "1234"
            };

            // Act
            var response = await _controllerTest.LoginAsync(userLogin);

            var result = response as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(msg, result.Value);
        }

        [TestMethod]
        public async Task LoginAsync_WhenUserPasswordIsIncorrect_ShouldReturn_BadRequestWhitExceptionMessage()
        {
            // Arrange
            CleanUp();
            BuildContext();
            var msg = "Usuario o contraseña incorrectos.";

            var userLogin = new UserLoginRequestDTO
            {
                Email = "daniel@email.com",
                Password = "12345"
            };

            // Act
            var response = await _controllerTest.LoginAsync(userLogin);

            var result = response as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(msg, result.Value);
        }

        [TestMethod]
        public async Task LoginAsyncModelState_WhenDataTypeIsIncorrect_ShouldReturn_ModelStateInvalid()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var userLogin = new UserLoginRequestDTO
            {
                Email = "marcos",
                Password = "123"
            };

            var context = new ValidationContext(userLogin, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UserLoginRequestDTO), typeof(UserLoginRequestDTO)), typeof(UserLoginRequestDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(userLogin, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
        }

        #endregion Login Tests

        #region Register Tests

        [TestMethod]
        public async Task RegisterAsync_WhenUserDoesNotExists_ShouldReturn_StatusOKAndUserLoggedInfo()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var userRegister = new UserRegistrationDTO
            {
                FirstName = "Pablo",
                LastName = "Medina",
                Email = "pablo@gmail.com",
                Password = "1234"
            };

            // Act
            var response = await _controllerTest.RegisterAsync(userRegister);
            var result = response as OkObjectResult;

            var userLoginInfo = result.Value as UserLoginResponseDTO;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(userRegister.Email, userLoginInfo.Email);
            Assert.AreEqual(userRegister.FirstName, userLoginInfo.FirstName);
            Assert.AreEqual(userRegister.LastName, userLoginInfo.LastName);
        }

        [TestMethod]
        public async Task RegisterAsync_WhenUserExists_ShouldReturn_BadRequestWhitMessage()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var userRegister = new UserRegistrationDTO
            {
                FirstName = "Daniel",
                LastName = "Bejar",
                Email = "daniel@email.com",
                Password = "1234"
            };

            var msg = $"El usuario con el email {userRegister.Email} ya esta en uso.";

            // Act
            var response = await _controllerTest.RegisterAsync(userRegister);
            var result = response as BadRequestObjectResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(msg, result.Value);
        }

        [TestMethod]
        public async Task RegisterAsyncModelState_WhenDataTypeIsIncorrect_ShouldReturn_ModelStateInvalid()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var userRegister = new UserRegistrationDTO
            {
                FirstName = "Daniel",
                LastName = "",
                Email = "daniel",
                Password = ""
            };

            var context = new ValidationContext(userRegister, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(UserRegistrationDTO), typeof(UserRegistrationDTO)), typeof(UserRegistrationDTO));

            // Act
            var isModelStateValid = Validator.TryValidateObject(userRegister, context, results, true);

            // Assert
            Assert.IsFalse(isModelStateValid);
        }

        #endregion Register Tests

        #region Profile Tests

        [TestMethod]
        public async Task ProfileAsync_WhenUserExists_ShouldReturn_StatusOKAndUserProfileInfo()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var profileExpected = new UserProfileDTO
            {
                FirstName = "Daniel",
                LastName = "Bejar",
                Email = "daniel@email.com",
                Photo = "Photo Daniel",
                Role = "Administrator"
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, "daniel@email.com") }));

            _controllerTest.ControllerContext = new ControllerContext();

            _controllerTest.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var response = await _controllerTest.ProfileAsync();

            var result = response as OkObjectResult;

            var profileResult = result.Value as UserProfileDTO;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(profileExpected.FirstName, profileResult.FirstName);
            Assert.AreEqual(profileExpected.LastName, profileResult.LastName);
            Assert.AreEqual(profileExpected.Email, profileResult.Email);
            Assert.AreEqual(profileExpected.Photo, profileResult.Photo);
            Assert.AreEqual(profileExpected.Role, profileResult.Role);
        }

        [TestMethod]
        public async Task ProfileAsync_WhenUserDoesNotExists_ShouldReturn_BadRequest()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Email, "matias@email.com") }));

            _controllerTest.ControllerContext = new ControllerContext();

            _controllerTest.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var response = await _controllerTest.ProfileAsync();

            var result = response as StatusCodeResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task ProfileAsync_WhenClaimsDoesNotExists_ShouldReturn_BadRequest()
        {
            // Arrange
            CleanUp();
            BuildContext();

            var user = new ClaimsPrincipal(new ClaimsIdentity());

            _controllerTest.ControllerContext = new ControllerContext();

            _controllerTest.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            // Act
            var response = await _controllerTest.ProfileAsync();

            var result = response as StatusCodeResult;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
        }

        #endregion Profile Tests

        #region Seed

        private void SeedUsers(ApplicationDbContext context)
        {
            var roleAdmin = new Role
            {
                Id = 1,
                Name = "Administrator",
                Description = "Description User Admin",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var roleStandard = new Role
            {
                Id = 2,
                Name = "Standard",
                Description = "Description User Standard",
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var userAdmin = new User
            {
                Id = 1,
                FirstName = "Daniel",
                LastName = "Bejar",
                Email = "daniel@email.com",
                Password = Encrypt.GetSHA256("1234"),
                Photo = "Photo Daniel",
                RoleId = 1,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var userStandard = new User
            {
                Id = 2,
                FirstName = "Lucas",
                LastName = "Gomez",
                Email = "lucas@email.com",
                Password = Encrypt.GetSHA256("1234"),
                Photo = "Photo Lucas",
                RoleId = 2,
                IsDeleted = false,
                CreatedAt = DateTime.UtcNow
            };

            _context.Add(roleAdmin);
            _context.Add(roleStandard);
            _context.Add(userAdmin);
            _context.Add(userStandard);
            _context.SaveChanges();
        }

        #endregion Seed
    }
}