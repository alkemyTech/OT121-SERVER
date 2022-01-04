using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.ActivitiesDTOs;
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
    public class ActivitiesServices : IActivitiesServices
    {

        #region Object and Constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly EntityMapper _mapper;
        private readonly IImageService _imageServices;
        public ActivitiesServices(IUnitOfWork unitOfWork, IImageService imageServices)
        {
            _unitOfWork = unitOfWork;
            _mapper = new EntityMapper();
            _imageServices = imageServices;
        }
        #endregion
        public async Task<IEnumerable<ActivitiesDTO>> GetAll()
        {
            var activitiesList = await _unitOfWork.ActivitiesRepository.GetAll();
            var activitiesDTO = activitiesList.Select(x => _mapper.FromActivitiesToActivitiesDTO(x)).ToList();

            return activitiesDTO;
        }
        public async Task<ActivitiesDTO> GetById(int id)
        {
            var activities = await _unitOfWork.ActivitiesRepository.GetById(id);
            var activitiesDTO = _mapper.FromActivitiesToActivitiesDTO(activities);

            return activitiesDTO;
        }

        public bool EntityExists(int id)
        {
            return _unitOfWork.ActivitiesRepository.EntityExists(id);
        }


        #region Update all data from Activities by PUT
        public async Task<Activities> UpdatePutAsync(ActivitiesUpdateDTO activitiesUpdateDto)
        {
            var activities = await _unitOfWork.ActivitiesRepository.GetById(activitiesUpdateDto.Id);
            if (activities != null)
            {
                _unitOfWork.DiscardChanges();
                await _imageServices.Delete(activities.Image);
                var urlImage = await _imageServices.SaveImageAsync($"{Guid.NewGuid()}_{activitiesUpdateDto.Image.FileName}", activitiesUpdateDto.Image);

                activities = _mapper.FromActivitiesUpdateDTOtoActivities(activitiesUpdateDto,urlImage);
               

                try
                {
                    await _unitOfWork.ActivitiesRepository.Update(activities);
                    await _unitOfWork.SaveChangesAsync();
                    return activities;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }
        #endregion
    }
}
