using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Helper.S3;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _ImageService;
        private readonly IUriService _uriService;
        private readonly EntityMapper _mapper;

        public MemberServices(IUnitOfWork unitOfWork, IImageService ImageService, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _ImageService = ImageService;
            _uriService = uriService;
            _mapper = new EntityMapper();
        }

        public async Task<List<MembersDTO>> GetAllAsync()
        {
            var members = await _unitOfWork.MemberRepository.GetAll();

            if (members == null)
                throw new Exception("No se encontraron datos.");

            var membersDto = members.Select(m => _mapper.FromMembersToMembersDto(m)).ToList();

            return membersDto;
        }
    }
}