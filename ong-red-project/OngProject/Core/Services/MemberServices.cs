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

            if (!members.Any())
                throw new Exception();

            var membersDto = members.Select(m => _mapper.FromMembersToMembersDto(m)).ToList();

            return membersDto;
        }

        public async Task<PaginationDTO<MembersDTO>> GetAllByPaginationAsync(int page)
        {
            int quantity = 10;
            var prevPage = string.Empty;
            var nextPage = string.Empty;
            var membersCount = await _unitOfWork.MemberRepository.CountAsync();
            var totalPages = (int)Math.Floor((decimal)membersCount / quantity);
            if ((membersCount % quantity) > 0)
                totalPages++;

            if (page > totalPages || page == 1)
                page = 1;
            else
                prevPage = _uriService.GetPage("/Member", page - 1);

            if (page < totalPages)
                nextPage = _uriService.GetPage("/Member", page + 1);


            var membersList = await _unitOfWork.MemberRepository.GetPageAsync(x => x.Id > 0, quantity, page);

            var membersDto = membersList.Select(m => _mapper.FromMembersToMembersDto(m)).ToList();

            PaginationDTO<MembersDTO> pagingResponse = new()
            {
                CurrentPage = page,
                TotalItems = membersCount,
                TotalPages = totalPages,
                PrevPage = prevPage,
                NextPage = nextPage,
                Items = membersDto.ToList()
            };
            return pagingResponse;
        }

        public async Task<Result> UpdateAsync(MemberUpdateDTO memberUpdate)
        {
            var member = await _unitOfWork.MemberRepository.GetById(memberUpdate.Id);

            if (member == null)
                return new Result().Fail($"El registro {memberUpdate.Id} no fue encontrado.");

            if (memberUpdate.Image != null)
            {
                try
                {
                    var nameImage = Guid.NewGuid();
                    var imgResult = await _imageService.Save(nameImage.ToString(), memberUpdate.Image);
                    member.Image = imgResult.Messages[0];
                }
                catch (Exception ex)
                {
                    return new Result().Fail($"Ocurrio un error al momento de intentar actualizar la imagen - {ex.Message}");
                }
            }

            member.Name = memberUpdate.Name;
            member.FacebookUrl = memberUpdate.FacebookUrl;
            member.InstagramUrl = memberUpdate.InstagramUrl;
            member.LinkedinUrl = memberUpdate.LinkedinUrl;
            member.Description = memberUpdate.Description;

            var updateResult = await _unitOfWork.MemberRepository.Update(member);

            if (updateResult == null)
            {
                return new Result().Fail("Ocurrio un error al intentar actualizar el miembro.");
            }

            await _unitOfWork.SaveChangesAsync();    

            return new Result().Success("Miembro actualizado con éxito.");
        }
    }
}
