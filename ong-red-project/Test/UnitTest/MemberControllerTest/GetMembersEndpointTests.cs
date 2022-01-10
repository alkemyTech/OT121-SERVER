using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Controllers;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Infrastructure.Data;
using Xunit;

namespace Test.UnitTest.MemberTest
{
    public class GetMembersEndpointTests
    {
        private MemberController _controller;
        private ApplicationDbContext _stubContext;
        private FakeMemberService _stubService;
        public GetMembersEndpointTests()
        {
            _stubContext = FakeDbContext.GetContext();
            _stubService = new FakeMemberService(_stubContext);
            _controller = new MemberController(_stubService);
            var regularUser = new ClaimsPrincipal(new ClaimsIdentity()); 
            _controller.ControllerContext = new ControllerContext(); 
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = regularUser }; 
        }

        [Fact(DisplayName ="Get Members by valid page number returns status code 200 and required members")]        
        public async Task GetAllMembersWithValidPageReturnsStatus200AndMembers()
        {
            var pageOne = 1;

            var createdResponse = await _controller.GetAll(pageOne);

            Assert.IsType<ObjectResult>(createdResponse);
            var result = createdResponse as ObjectResult;
            Assert.NotNull(result);
            var response = result.Value as ResultValue<PaginationDTO<MembersDTO>>;
            Assert.NotNull(response);
            var resultStatusCode = response.StatusCode;
            Assert.Equal(200, resultStatusCode);
            PaginationDTO<MembersDTO> responsePage = response.Value;
            List<MembersDTO> membersList = responsePage.Items;
            Assert.Equal(7, membersList.Count);
        }

        [Fact(DisplayName ="Get Members by inexistent page returns status code 400")]
        public async Task GetMembersByInexistentPageReturnsStatus400()
        {
            var pageTen = 10;

            var createdResponse = await _controller.GetAll(pageTen);

            Assert.IsType<ObjectResult>(createdResponse);
            var result = createdResponse as ObjectResult;
            Assert.NotNull(result);
            var response = result.Value as ResultValue<PaginationDTO<MembersDTO>>;
            Assert.NotNull(response);
            var resultStatusCode = response.StatusCode;
            Assert.Equal(400, resultStatusCode);
            string responseMessage = response.Messages[0];
            Assert.Equal("No existe la página proporcionada.", responseMessage);
        }

        [Fact(DisplayName ="Get Members with no page number returns status code 403")]
        public async Task GetMembersWithNoPageReturnsStatus403()
        {
            int? nullPage = null;

            var createdResponse = await _controller.GetAll(nullPage);
                
            Assert.IsType<ObjectResult>(createdResponse);
            var result = createdResponse as ObjectResult;
            Assert.NotNull(result);
            var resultStatusCode = result.StatusCode;
            Assert.Equal(403, resultStatusCode);
        }
    }
}

