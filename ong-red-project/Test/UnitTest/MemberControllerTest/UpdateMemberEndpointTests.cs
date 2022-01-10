using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using Xunit;

namespace Test.UnitTest.MemberTest
{
    public class UpdateMemberEndpointTests
    {
        private Mock<IMemberServices> _mock;
        private MemberUpdateDTO _validMember;
        private MemberUpdateDTO _memberWithId100;
        private MemberUpdateDTO _memberWithoutName;
        
        public UpdateMemberEndpointTests()
        {
            _mock = new Mock<IMemberServices>();
            _validMember = new MemberUpdateDTO()
            {
                Id = 1,
                Name = "jose",
            };

            _memberWithoutName = new MemberUpdateDTO()
            {
                Id = 1,
            };

            _memberWithId100 = new MemberUpdateDTO()
            {
                Id = 100,
                Name = "jose",
            };

        }
        [Fact(DisplayName ="Update valid member should return Ok")]
        public async Task UpdateMemberWithValidIdShouldReturnOk()
        {
            _mock.Setup(m => m.UpdateAsync(_validMember)).ReturnsAsync(new Result().Success("Miembro actualizado con éxito.")); 
            var controller = new MemberController(_mock.Object);

            var createdResponse = await controller.Update(_validMember, 1);

            Assert.IsType<OkObjectResult>(createdResponse);
            var result = createdResponse as OkObjectResult;
            Assert.NotNull(result);
            Result response = result.Value as Result;
            Assert.NotNull(response);
            var responseMessage = response.Messages[0];
            Assert.Equal("Miembro actualizado con éxito.", responseMessage);
        }

        [Fact(DisplayName = "Update member with invalid Id should return BadRequest")]
        public async Task UpdateMemberWithInvalidIdShouldReturnBadRequest()
        {
            _mock.Setup(m => m.UpdateAsync(_memberWithId100)).ThrowsAsync(new Exception("el registro no fue encontrado")); 
            var controller = new MemberController(_mock.Object);

            var createdResponse = await controller.Update(_memberWithId100, 100);

            Assert.IsType<BadRequestObjectResult>(createdResponse);
            var result = createdResponse as BadRequestObjectResult;
            Assert.NotNull(result);
            Result response = result.Value as Result;
            Assert.NotNull(response);
            var responseMessage = response.Messages[0];
            Assert.Equal("Ocurrio un error al momento de intentar actulizar los datos - el registro no fue encontrado", responseMessage);
        }

        [Fact(DisplayName ="Update member with invalid data should return BadRequest")]
        public async Task UpdateMemberWithInvalidDataShouldReturnBadRequest()
        {
            _mock.Setup(m => m.UpdateAsync(_memberWithoutName));
            var controller = new MemberController(_mock.Object);
            controller.ModelState.AddModelError("Name","Required");

            var createdResponse = await controller.Update(_memberWithoutName, 1);

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