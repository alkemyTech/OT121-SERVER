using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Configuration;
using Test.Helper;

namespace Test.UnitTest.TestimonialsControllerTest
{
    [TestClass]
    public class TestimonialsControllerTest
    {
        #region Objects and Constructor

        private IConfiguration _configuration;

        private IOptions<AWSSettings> _awsSettings;

        private ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;
        private ITestimonialsServices _testimonialsServices;
        private TestimonialsController _controllerTest;
        private IUriService _uriService;
        private IImageService _imageServices;
        private EntityMapper _mapper;

        private readonly string _baseUri = "https://localhost:44353";

        [TestInitialize]
        public void BuildContext()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .AddUserSecrets<Secrets>()
                .Build();
            //Settings Secrets
            _awsSettings = Options.Create(_configuration.GetSection("AWSSettings").Get<AWSSettings>());
            ///

            _context = ApplicationDbContextInMemory.GetContext();

            _unitOfWork = new UnitOfWork(_context);

            _imageServices = new ImageService(_awsSettings);

            _uriService = new UriService(_baseUri);

            _testimonialsServices = new TestimonialsServices(_unitOfWork, _imageServices, _uriService);

            _controllerTest = new TestimonialsController(_testimonialsServices);

            SeedTestimonials(_context);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
        }

        #endregion Objects and Constructor

        #region GetAll Test

        [TestMethod]
        public async Task GetAllAsyncPaging_WithExistingPageNumber_ShouldReturn_StatusOKAndNumberOfPages_Items_TotalItems()
        {
            //Arrange
            CleanUp();
            BuildContext();

            int page = 1;

            // Act
            var response = await _controllerTest.GetAllAsync(page);
            var result = response as OkObjectResult;
            var expected = result.Value as PaginationDTO<TestimonialsDTO>;
            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(10, expected.TotalPages);
            Assert.AreEqual(10, expected.Items.Count());
            Assert.AreEqual(100, expected.TotalItems);
        }

        [TestMethod]
        public async Task GetAllAsyncPaging_WithNonExistenPageNumber_ShouldReturn_BadRequestAndMessage()
        {
            //Arrange
            CleanUp();
            BuildContext();

            var msg = "La pagina no existente.";

            int page = 20;

            // Act
            var response = await _controllerTest.GetAllAsync(page);
            var result = response as BadRequestObjectResult;
            var msgResponse = result.Value as Result;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(msg, msgResponse.Messages[0]);
        }

        #endregion GetAll Test

        #region Create Tests

        [TestMethod]
        public async Task Create()
        {
            //Arrange
            CleanUp();
            BuildContext();

            var msg = "Datos guardados satisfactoriamente.";

            var newTestimonial = new TestimonialsCreateDTO
            {
                Name = "Testimonial Test",
                Image = GetImage(),
                Content = "Content for testimonial test"
            };

            //Act
            var response = await _controllerTest.Create(newTestimonial);
            var result = response as ObjectResult;
            var msgResponse = result.Value as Result;

            // Assert
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual(msg, msgResponse.Messages[0]);
        }

        #endregion Create Tests

        #region Update

        [TestMethod]
        public async Task Update()
        {
            //Arrange
            CleanUp();
            BuildContext();

            var msg = "Testimonio actualizado con éxito.";

            int id = 1;

            var updateTestimonial = new TestimonialsUpdateDTO()
            {
                Id = 1,
                Name = "Testimonial Update Test",
                Image = GetImage(),
                Content = "Content for testimonial update test",
            };

            //Act
            var response = await _controllerTest.Update(updateTestimonial, id);
            var result = response as OkObjectResult;
            var msgResponse = result.Value as Result;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(msg, msgResponse.Messages[0]);
        }

        [TestMethod]
        public async Task Update_IdsDoNotMatch()
        {
            //Arrange
            CleanUp();
            BuildContext();

            var msg = "Los ids deben coincidir.";

            int id = 2;

            var updateTestimonial = new TestimonialsUpdateDTO()
            {
                Id = 1,
                Name = "Testimonial Update Test",
                Image = GetImage(),
                Content = "Content for testimonial update test",
            };

            //Act
            var response = await _controllerTest.Update(updateTestimonial, id);
            var result = response as BadRequestObjectResult;
            var msgResponse = result.Value as Result;

            // Assert
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual(msg, msgResponse.Messages[0]);
        }

        [TestMethod]
        public async Task Update_TestimonialIdWasNotFound()
        {
            //Arrange
            CleanUp();
            BuildContext();

            int id = 200;

            var updateTestimonial = new TestimonialsUpdateDTO()
            {
                Id = 200,
                Name = "Testimonial Update Test",
                Image = GetImage(),
                Content = "Content for testimonial update test",
            };

            var msg = $"El registro {updateTestimonial.Id} no fue encontrado.";

            //Act
            var response = await _controllerTest.Update(updateTestimonial, id);
            var result = response as OkObjectResult;
            var msgResponse = result.Value as Result;

            // Assert
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsTrue(msgResponse.HasErrors);
            Assert.AreEqual(msg, msgResponse.Messages[0]);
        }

        #endregion Update

        #region Methods

        private void SeedTestimonials(ApplicationDbContext context)
        {
            for (int i = 1; i <= 100; i++)
            {
                var testimonials = new Testimonials
                {
                    Name = $"Testimonial {i}",
                    Image = $"Image{i}.jpg",
                    Content = $"Content for testimonial {i}"
                };

                context.Add(testimonials);
            }

            context.SaveChanges();
        }

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

        #endregion Methods
    }
}