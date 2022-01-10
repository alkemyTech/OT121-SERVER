using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Infrastructure.Data;
using Xunit;

namespace Test.UnitTest.MemberTest
{
    public class CreateMemberEndpointTests
    {
        private ApplicationDbContext _stubContext;
        private FakeMemberService _stubService;
        private MemberInsertDTO _validMember;
        private MemberInsertDTO _invalidImageMember;
        private MemberInsertDTO _invalidMember;

        public CreateMemberEndpointTests()
        {
            _stubContext = FakeDbContext.GetContext();
            _stubService = new FakeMemberService(_stubContext);
            _validMember = new MemberInsertDTO()
            {
                Name = "Emma",
                Image = new FormFile(new MemoryStream(), 0, 100, "validImage", "validImage.png")
            };  

            _invalidImageMember = new MemberInsertDTO()
            {
                Name = "Emma",
                Image = new FormFile(new MemoryStream(), 0, 100, "invalidImage", "invalidImage.pdf")
            };  

            _invalidMember = new MemberInsertDTO()
            {
                Image = new FormFile(new MemoryStream(), 0, 100, "validImage", "validImage.png")
            };  
        }

        [Fact(DisplayName= "Create valid Member should return Ok and update database")]
        public async Task CreateMemberWithValidDataShouldReturnOkAndUpdateBase()
        {
            var controller = new MemberController(_stubService);
            MemberInsertDTO validMember = _validMember;

            var createdResponse = await controller.Create(validMember);

            Assert.IsType<OkObjectResult>(createdResponse);
            var result = createdResponse as OkObjectResult;
            Assert.NotNull(result);
            Result response = result.Value as Result;
            Assert.NotNull(response);
            var responseMessage = response.Messages[0];
            Assert.Equal("Datos guardados satisfactoriamente.", responseMessage);
        }
        
        [Fact(DisplayName= "Create Member with invalid image should return BadResult and no update database")]
        public async Task CreateMemberWithInvalidImageShouldFail()
        {
            var controller = new MemberController(_stubService);
            MemberInsertDTO newMember = _invalidImageMember;

            var createdResponse = await controller.Create(newMember);

            Assert.IsType<BadRequestObjectResult>(createdResponse);
            var result = createdResponse as BadRequestObjectResult;
            Assert.NotNull(result);
            Result response = result.Value as Result;
            Assert.NotNull(response);
            var responseMessage = response.Messages[0];
            Assert.Equal("Ocurrio un error al momento de intentar ingresar los datos - extension no valida", responseMessage);
        }
        
        [Fact(DisplayName= "Create Member with invalid data should return BadResult and no update database")]
        public async Task CreateInvalidMemberShouldReturnBadRequestWithoutUpdateDatabase()
        {
            var controller = new MemberController(_stubService);
            MemberInsertDTO noNameMember = _invalidMember;
            controller.ModelState.AddModelError("Name","Required");
            
            var createdResponse = await controller.Create(noNameMember);

            Assert.IsType<BadRequestObjectResult>(createdResponse);
            var result = createdResponse as BadRequestObjectResult;
            Assert.NotNull(result);
            Result response = result.Value as Result;
            Assert.NotNull(response);
            var responseMessage = response.Messages[0];
            Assert.Equal("Algo salio mal.", responseMessage);
        }
    }
}