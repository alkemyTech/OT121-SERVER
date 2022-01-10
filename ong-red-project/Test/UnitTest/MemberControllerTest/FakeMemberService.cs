using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Infrastructure.Data;

namespace Test.UnitTest.MemberTest
{
    public class FakeMemberService : IMemberServices
    {
        private ApplicationDbContext _dbContext;
        public FakeMemberService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result> CreateAsync(MemberInsertDTO newMember)
        {
            var newRecord = new Member
            {
                Name = newMember.Name,
                FacebookUrl = newMember.FacebookUrl,
                InstagramUrl = newMember.InstagramUrl,
                LinkedinUrl = newMember.LinkedinUrl,
                Description = newMember.Description
            };

            var isPng = newMember.Image.FileName.EndsWith(".png");
            if(isPng)
            {
                await _dbContext.Members.AddAsync(newRecord);
                await _dbContext.SaveChangesAsync();
                return new Result().Success("Datos guardados satisfactoriamente.");
            }
            throw new Exception("extension no valida");
        }

        public Task<ResultValue<List<MembersDTO>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ResultValue<PaginationDTO<MembersDTO>>> GetAllByPaginationAsync(int page)
        {
            List<MembersDTO> x = new List<MembersDTO>{null, null, null, null, null, null, null};

            var newPage = new PaginationDTO<MembersDTO>()
            {
                Items = x
            };
            await Task.Delay(10);
            if(page == 1)
            {
                return new ResultValue<PaginationDTO<MembersDTO>>()
                {
                    StatusCode = 200,
                    Value = newPage
                };
            }
            
            return new ResultValue<PaginationDTO<MembersDTO>>() {
                StatusCode = 400, 
                Messages = new List<string>() { "No existe la página proporcionada." } 
            };
        }

        public Task<Result> UpdateAsync(MemberUpdateDTO memberUpdate)
        {
            throw new NotImplementedException();
        }

    }
}