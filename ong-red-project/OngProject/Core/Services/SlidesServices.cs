using Microsoft.AspNetCore.Http;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.FomFileData;
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
    public class SlidesServices : ISlidesServices
    {
        #region Object and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageServices;

        private readonly EntityMapper _mapper;

        public SlidesServices(IUnitOfWork unitOfWork, IImageService imageServices)
        {
            _unitOfWork = unitOfWork;
            _imageServices = imageServices;
            _mapper = new EntityMapper();
        } 
        #endregion


        public bool EntityExist(int id)
        {
            return _unitOfWork.SlidesRepository.EntityExists(id);
        }
        
        

        public async Task<List<SlideDataShortResponse>> GetListOfSlides()
        {
            var slidesList = await _unitOfWork.SlidesRepository.GetAll();

            var slides = slidesList.Select(s => _mapper.FromSlidesToSlidesShortResponseDTO(s)).ToList();

            return slides;
        }

        public async Task<SlideDataFullResponse> GetSlideById(int id)
        {    
            var slide = await _unitOfWork.SlidesRepository.GetById(id);
            if(slide == null)
                return null;
            return _mapper.FromSlideToSlidesFullResponseDTO(slide);
        }


    }
}
