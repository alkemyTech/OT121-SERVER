using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using Xunit;

namespace Test.UnitTest.OrganizationControllerTests
{
    public class GetOrganizationEndpointTests
    {
        private Mock<IOrganizationsServices> _mock;
        public GetOrganizationEndpointTests()
        {
            _mock = new Mock<IOrganizationsServices>();
        }

        [Fact(DisplayName ="Get organization by valid id should return Ok and requested organization")]
        public async Task GetOrganizationByValidIdShouldReturnOkResultAndOrganization()
        {
            OrganizationsGetDTO somosMasOrganization = new OrganizationsGetDTO()
            {
                Name = "somos mas",
                Image = "sms.png"
            };
            var somosMasId = 1;
            var contextWithSomosMas = _mock;
            contextWithSomosMas.Setup(m => m.EntityExists(somosMasId)).Returns(true); 
            contextWithSomosMas.Setup(m => m.GetById(somosMasId)).ReturnsAsync(somosMasOrganization); 
            var controller = new OrganizationsController(contextWithSomosMas.Object);

            var createdResponse = await controller.GetPublic(somosMasId);

            Assert.IsType<OkObjectResult>(createdResponse);
            var result = createdResponse as OkObjectResult;
            Assert.NotNull(result);
            OrganizationsGetDTO response = result.Value as OrganizationsGetDTO;
            Assert.NotNull(response);
            var responseName = response.Name;
            Assert.Equal("somos mas", responseName);
        }

        [Fact(DisplayName ="Get organization with inexistent id should return NoFound")]
        public async Task GetOrganizationWhitInexistentIdShouldReturnNotFound()
        {
            var contextWithoutOrganizations = _mock;
            contextWithoutOrganizations.Setup(m => m.EntityExists(1)).Returns(false); 
            var controller = new OrganizationsController(contextWithoutOrganizations.Object);

            var createdResponse = await controller.GetPublic(1);

            Assert.IsType<NotFoundResult>(createdResponse);
        }


    }
}