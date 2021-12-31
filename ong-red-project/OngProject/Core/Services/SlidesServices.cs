using Microsoft.AspNetCore.Http;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.SlidesDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Base64ImageInspector;
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

        public async Task<int> CreateSlideAsync(SlideDTO model)
        {
            if(model.Order ==null)
                await SetOrderAsTheLastExistentAsync(model);

            Slides slide = _mapper.FromEntryDTOtoSlide(model);

            Result resultFromAws = await UploadEncodedImageToBucketAsync(model.Base64Image);
            
            if(resultFromAws.HasErrors == false)
                slide.ImageUrl = resultFromAws.Messages[0];    

            await _unitOfWork.SlidesRepository.Insert(slide);
            await _unitOfWork.SaveChangesAsync();
            return slide.Id;
        }

        private string GetNewImageName(string imageType)
        {
            TimeSpan elapsedTime = DateTime.Now - DateTime.UnixEpoch;
            var timestamp = (long) elapsedTime.TotalSeconds;
            return "slide_" + timestamp + '.' + imageType;
        }

        private async Task<Result> UploadEncodedImageToBucketAsync(string rawBase64File) 
        {
            Base64ImageInspector.SplitIntoTypeAndImageData(rawBase64File, out string contentType, out string imageType, out string base64ImageData);
            string newName = GetNewImageName(imageType);
            var formFileData = new FormFileData(){
                FileName = newName,
                ContentType = contentType,
                Name = newName
            };
            byte[] imageBinaryFile = Convert.FromBase64String(base64ImageData);
            IFormFile newFile = ConvertFile.BinaryToFormFile(imageBinaryFile, formFileData);
            return await _imageServices.Save(newFile.FileName, newFile);
        }
        
        private async Task SetOrderAsTheLastExistentAsync(SlideDTO model)
        {
            IEnumerable<Slides> slides = await _unitOfWork.SlidesRepository.GetAll();
            var maxOrder = slides.Select(s => s.Order).Max();
            model.Order = maxOrder + 1;
        }
  
    }
}
