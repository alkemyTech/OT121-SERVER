using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Interfaces.IServices.AWS;
using OngProject.Core.Mapper;
using OngProject.Infrastructure.Repositories;
using OngProject.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Core.Services
{
    public class UserServices : IUserServices
    {
        #region Object and Constructor

        private readonly IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        private readonly IImageService _imageServices;
        private readonly EntityMapper _mapper;

        public UserServices(IUnitOfWork _unitOfWork, IConfiguration configuration, IImageService imageServices, EntityMapper mapper)
        {
            this._unitOfWork = _unitOfWork;
            _configuration = configuration;
            _imageServices = imageServices;
            _mapper = mapper;
        }

        #endregion Object and Constructor

        #region Methods

        public async Task<UserRegistrationDTO> RegisterAsync(UserRegistrationDTO user)
        {
            var newUser = _mapper.FromUserRegistrationDtoToUser(user);

            var result = await _unitOfWork.UsersRepository.Insert(newUser);

            return _mapper.FromUserToUserRegistrationDto(result);
        }

        #endregion Methods
    }
}