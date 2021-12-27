using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Common;
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
using System.Text;
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
        private readonly JWTSettings _jWTSettings;

        public UserServices
            (IUnitOfWork _unitOfWork,
            IConfiguration configuration,
            IImageService imageServices,
            IOptions<JWTSettings> jWTSettings)
        {
            this._unitOfWork = _unitOfWork;
            _configuration = configuration;
            _imageServices = imageServices;
            _mapper = new EntityMapper();
            _jWTSettings = jWTSettings.Value;
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

            JwtSecurityToken jwtSecurityToken = GenerateJWTToken(userLogin);

            UserLoginResponseDTO response = new()
            {
                Id = userLogin.Id,
                FirstName = userLogin.FirstName,
                LastName = userLogin.LastName,
                Email = userLogin.Email,
                Photo = userLogin.Photo,
                Role = userLogin.Role.Name,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

            return response;
        }

        #endregion Methods

        #region Private Methods (Token)

        private JwtSecurityToken GenerateJWTToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Password", user.Password),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jWTSettings.DurationInDays),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        #endregion Private Methods (Token)   

        public async Task<UserProfileDTO> UserExistsByEmail(string email)
        {
            var result = await _unitOfWork.UsersRepository.GetByEmail(email);
            if (result == null)
                return null;

            UserProfileDTO response = new()
            {
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                Photo = result.Photo,
                Role = result.Role.Name
            };

            return response;
        }

        public async Task<bool> UserExistsById(int id)
        {
            var result = await _unitOfWork.UsersRepository.GetById(id);
            if (result != null)
                return true;
            return false;
        }

        #region Get all data from all users
        public async Task<IEnumerable<User>> GetUsersAllDataAsync()
        {
            return await _unitOfWork.UsersRepository.FindByCondition(x => x.Id > 0, y => y.Role);
        }
        #endregion

        #region Update user by id with jsonPatch
        public async Task<User> UpdatePatchAsync(int id, JsonPatchDocument patch)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);
            if (user != null)
            {
                try
                {
                    var pass = Encrypt.GetSHA256(user.Password);
                    patch.ApplyTo(user);
                    if (!Encrypt.Verify(user.Password, pass))
                        user.Password = Encrypt.GetSHA256(user.Password);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return user;
        }
        #endregion
    }

}