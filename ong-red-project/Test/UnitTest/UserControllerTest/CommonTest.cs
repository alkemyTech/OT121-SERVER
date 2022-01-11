using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OngProject.Controllers;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Services;
using OngProject.Infrastructure.Data;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using Test.Helper;
using Test.Interfaces;

namespace Test.UnitTest.UserControllerTest
{
    [TestClass]
    public abstract class CommonTest
    {
        #region Protected
        protected ApplicationDbContext _context;
        protected IConfiguration _configuration;
        protected IMailService _mailServices;
        protected IImageService _imageServices;
        protected IUriService _uriServices;
        protected IUnitOfWork unitOfWork;
        IOptions<OngProject.Core.Entities.MailSettings> _mailSettings;
        IOptions<MailConstants> _mailConstants;
        protected IOptions<JWTSettings> _jWTSettings;
        #endregion

        #region Seed for Test

        private void RoleSeed(ApplicationDbContext context)
        {
            Role[] rols = new Role[]{
                new Role(){
                    Name = "Administrator",
                    Description = "Description for Administrator user test."
                },
                new Role(){
                    Name = "Standard",
                    Description = "Description for Standard user test."
                },
            };

            foreach (Role role in rols)
            {
                context.Add(role);
            }
        }
        private void UserSeed(ApplicationDbContext context)
        {
            for (int i = 1; i <= 30; i++)
            {
                User user = new User()
                {
                    Id = i,
                    FirstName = $"Name_{i}",
                    LastName = $"LastName_{i}",
                    Email = $"email_{i}@example.com",
                    Password = OngProject.Common.Encrypt.GetSHA256("12345"),
                    Photo = null,
                    RoleId = i % 2 == 1 ? 1 : 2,
                    CreatedAt = DateTime.Now
                };
                context.Add(user);
            }
        }
        private void BaseSeed(ApplicationDbContext context)
        {
            UserSeed(context);
            AddSeeds(context);
            context.SaveChanges();
        }
        #endregion

        #region Methods for Test
        public abstract void AddSeeds(ApplicationDbContext context);
        public abstract void AddServices();
        public abstract void AddControllers();
        #endregion
        
        #region Database
        private static ApplicationDbContext MakeContext(string dataBaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(dataBaseName)
                                .Options;
            var context = new ApplicationDbContext(options);

            return context;
        }
        #endregion

        [TestInitialize]
        #region StartTest Settings

        public void MakeArrange()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .AddUserSecrets<ITestSecrets>()
                .Build();

            _mailConstants = Options.Create(_configuration.GetSection("MailConstants").Get<MailConstants>());
            _mailSettings = Options.Create(_configuration.GetSection("MailSettings").Get<MailSettings>());
            _jWTSettings = Options.Create(_configuration.GetSection("JWTSettings").Get<JWTSettings>());


            _context = MakeContext("DbTest");

            unitOfWork = new UnitOfWork(_context);
            _mailServices = new MailService(_mailSettings, _mailConstants);

            AddServices();
            AddControllers();
            
            BaseSeed(_context);
        }
        #endregion

        [TestCleanup]
        #region EndTest Settings
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }
        #endregion
    }
}
