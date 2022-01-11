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

namespace Test.UnitTest.ContactsControllerTest
{
    [TestClass]
    public class CommonTest
    {
        private ApplicationDbContext _context;
        public ContactsController contactsController;
        private IConfiguration _configuration;
        private IContactsServices _contactsServices;
        private IMailService _mailService;
        private IUriService _uriServices;
        private IUnitOfWork unitOfWork;
        IOptions<OngProject.Core.Entities.MailSettings> _mailSettings;
        IOptions<MailConstants> _mailConstants;

        public static ApplicationDbContext MakeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                                .Options;
            var context = new ApplicationDbContext(options);

            return context;
        }

        [TestInitialize]
        public void MakeArrange()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();
            _context = MakeContext();
            _mailConstants = Options.Create(new MailConstants()
            {
                PathTemplate = "Templates/htmlpage.html",
                ReplaceMailTitle = "{mail_title}",
                ReplaceMailBody = "{mail_body}",
                ReplaceMailContact = "{mail_contact}",
                ReplaceMailConfirm = "{mail_confirm}",
                TitleMailConfirm = "Confirmacion de registro",
                WelcomeMailBody = "Bienvenido/a ",
                MailONG = "",
                ReplyToContact = ". Gracias por contactarnos, a la brevedad responderemos tu mensaje!!!",
                TitleMailContact = "Contacto WEB"
            });
            _mailSettings = Options.Create(new MailSettings()
            {
                ApiKey = "SENDGRID_API_KEY",
                SenderMail = "SENDGRID_SENDERS_VERIFIED",
                SenderName = "ONG"
            });
            _mailService = new MailService(_mailSettings, _mailConstants);
            unitOfWork = new UnitOfWork(_context);
            _contactsServices = new ContactsServices(unitOfWork, _uriServices);
            contactsController = new ContactsController(_contactsServices, _mailService);
            SeedContacts(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
        }

        #region Seed for test
        private void SeedContacts(ApplicationDbContext context)
        {
            var role = new Role
            {
                Name = "Administrator",
                Description = "Usuario administrador test"
            };
            var contactsList = new List<Contacts>() {
                new Contacts
                {
                    Id = 1,
                    Name = "Contact 1",
                    Phone = 381,
                    Email = "test1@test.com",
                    Message = "Message from contact ",
                    CreatedAt = DateTime.Now
                },
                new Contacts
                {
                    Id = 2,
                    Name = "Contact 2",
                    Phone = 3821,
                    Email = "test2@test.com",
                    Message = "Message from contact 2",
                    CreatedAt = DateTime.Now
                },
                new Contacts
                {
                    Id = 3,
                    Name = "Contact 3",
                    Phone = 38231,
                    Email = "test3@test.com",
                    Message = "Message from contact 3",
                    CreatedAt = DateTime.Now
                }
            };
            foreach (var item in contactsList)
                context.Add(item);
            context.Add(role);
            context.SaveChanges();
        }
        #endregion
    }
}
