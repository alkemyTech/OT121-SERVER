using OngProject.Common;
using OngProject.Core.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface IMemberServices
    {
        Task<List<MembersDTO>> GetAllAsync();

        Task<Result> CreateAsync(MemberInsertDTO newMember);

        Task<Result> UpdateAsync(MemberUpdateDTO memberUpdate);
    }
}