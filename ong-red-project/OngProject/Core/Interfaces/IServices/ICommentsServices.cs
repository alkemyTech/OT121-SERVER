using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Core.Interfaces.IServices
{
    public interface ICommentsServices
    {
        Task<Result> Delete(int id);
        Task<bool> ValidateCreatorOrAdmin(ClaimsPrincipal user, int id);
        public bool EntityExists(int id);
    }
}
