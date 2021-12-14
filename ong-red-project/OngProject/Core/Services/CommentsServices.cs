using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class CommentsServices : ICommentsServices
    
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUriService _uriService;
        public CommentsServices(IUnitOfWork unitOfWork, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _uriService = uriService;
        }


        public async Task<Result> Delete(int id)
        {
            var response = await _unitOfWork.CommentsRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return response;
        }

        public async Task<bool> ValidateCreatorOrAdmin(ClaimsPrincipal user, int id)
        {
            var userId = user.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var comment =  await _unitOfWork.CommentsRepository.GetById(id);
            if(comment.UserId.Equals(int.Parse(userId)) || user.IsInRole("Administrator")) return true;
            return false;
        }

        public bool EntityExists(int id)
        {
            return _unitOfWork.CommentsRepository.EntityExists(id);
        }
    }
}
