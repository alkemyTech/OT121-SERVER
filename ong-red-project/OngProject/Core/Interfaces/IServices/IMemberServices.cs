using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IMemberServices
    {
        Task<ResultValue<List<MembersDTO>>> GetAllAsync();
        Task<ResultValue<PaginationDTO<MembersDTO>>> GetAllByPaginationAsync(int page);

        Task<Result> CreateAsync(MemberInsertDTO newMember);

        Task<Result> UpdateAsync(MemberUpdateDTO memberUpdate);
    }
}