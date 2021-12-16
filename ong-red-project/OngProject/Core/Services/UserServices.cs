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

        public UserServices(IUnitOfWork _unitOfWork, IConfiguration configuration, IImageService imageServices)
        {
            this._unitOfWork = _unitOfWork;
            _configuration = configuration;
            _imageServices = imageServices;
            _mapper = new EntityMapper();
        }

        #endregion Object and Constructor

        #region Methods

        public async Task<UserRegistrationDTO> RegisterAsync(UserRegistrationDTO user)
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = Encrypt.GetSHA256(user.Password),
                RoleId = 2
            };

            var result = await _unitOfWork.UsersRepository.Insert(newUser);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.FromUserToUserRegistrationDto(result);
        }

        public async Task<UserLoginResponseDTO> LoginAsync(UserLoginRequestDTO user)
        {
            var userLogin = await _unitOfWork.UsersRepository.GetByEmail(user.Email);

            if (userLogin == null)
            {
                throw new Exception("Usuario o contraseña incorrectos.");
            }

            var result = Encrypt.Verify(user.Password, userLogin.Password);

            if (!result)
            {
                throw new Exception("Usuario o contraseña incorrectos.");
            }

            return _mapper.FromUserToUserLoginResponseDto(userLogin);
        }

        #endregion Methods
    }
}