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
        private readonly IImageService _imageService;
        private readonly IUriService _uriService;
        private readonly EntityMapper _mapper;

        public MemberServices(IUnitOfWork unitOfWork, IImageService ImageService, IUriService uriService)
        {
            _unitOfWork = unitOfWork;
            _imageService = ImageService;
            _uriService = uriService;
            _mapper = new EntityMapper();
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

            try
            {
                if (newMember.Image != null)
                {
                    var nameImage = Guid.NewGuid();
                    var result = await _imageService.Save(nameImage.ToString(), newMember.Image);
                    newRecord.Image = result.Messages[0];
                }
            }
            catch (Exception ex)
            {
                return new Result().Fail($"Ocurrio un error al momento de intentar guardar la imagen - {ex.Message}");
            }

            await _unitOfWork.MemberRepository.Insert(newRecord);

            await _unitOfWork.SaveChangesAsync();

            return new Result().Success("Datos guardados satisfactoriamente.");
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