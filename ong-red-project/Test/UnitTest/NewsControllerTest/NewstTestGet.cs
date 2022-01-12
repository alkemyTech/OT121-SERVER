using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Test.Helper;

namespace Test.UnitTest.ActivitiesControllerTest
{
    [TestClass]
    public class NewsTestGet : BaseTest
    {
        private ApplicationDbContext _context;
        private AuthController authController;
        private IConfiguration _configuration;

        private INewsServices _newsServices;
        private NewsController _newsController;
        private  IUriService _uriService;

        private IUserServices _userService;
        private IMailService _mailService;
        private IImageService _imageServices;
        private IOptions<JWTSettings> _jWTSettings;
        public object Request { get; private set; }
        [TestInitialize]
        public void MakeArrange()
        {
            var config = new Dictionary<string, string>{
                {"JWTSettings:Key", "cZNKtU191SkXZuqxe1amE6bpOqBKimG5"},
                {"JWTSettings:Issuer", "Team121" },
                {"JWTSettings:Audience", "Alkemy" },
                {"JWTSettings:DurationInDays", "1" },
                { "ApiKey", "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" },
                { "SenderMail", "dfr80@hotmail.com" },
                {"SenderName", "ONG" },
                { "PathTemplate", "Templates/htmlpage.html" },
                {"ReplaceMailTitle", "{mail_title}" },
                {"ReplaceMailBody", "{mail_body}" },
                {"ReplaceMailContact", "{mail_contact}" },
                {"ReplaceMailConfirm", "{mail_confirm}" },
                { "TitleMailConfirm", "Confirmacion de registro"},
                {"WelcomeMailBody", "Bienvenido/a "},
                {"MailONG", ""},
                { "ReplyToContact", ". Gracias por contactarnos, a la brevedad responderemos tu mensaje!!!"},
                { "TitleMailContact", "Contacto WEB"}
                };


            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(config)
                .Build();

            _jWTSettings = Options.Create(new JWTSettings()
            {
                Key = "cZNKtU191SkXZuqxe1amE6bpOqBKimG5",
                Issuer = "Team121",
                Audience = "Alkemy",
                DurationInDays = 1
            }
            );

            _context = MakeContext("TestDb");
            IUnitOfWork unitOfWork = new UnitOfWork(_context);

            _newsServices = new NewsServices(unitOfWork, _imageServices, _uriService);
            _newsController = new NewsController(_newsServices);

            _userService = new UserServices(unitOfWork, _configuration, _imageServices, _jWTSettings);
            authController = new AuthController(_userService, _mailService);
            SeedContacts(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task Get_News_Should_Return_Ok()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 1;
            //Act
            var response = await _newsController.GetAsync(id);
            var expected = response as OkObjectResult;
            // Assert
            Assert.AreEqual(200, expected.StatusCode);
        }

        [TestMethod]
        public async Task Get_News_Should_Return_Not_Found()
        {
            //Arrange
            Cleanup();
            MakeArrange();
            int id = 2;
            //Act
            var response = await _newsController.GetAsync(id);
            var expected = response as NotFoundResult;
            // Assert
            Assert.AreEqual(404, expected.StatusCode );
        }


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
    }
}
