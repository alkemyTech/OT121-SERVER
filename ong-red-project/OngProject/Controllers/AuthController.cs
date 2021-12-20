using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Object and Constructor

        private readonly IUserServices _userServices;
        private readonly IMailService _mailService;

        public AuthController(IUserServices _userServices, IMailService mailService)
        {
            this._userServices = _userServices;
            _mailService = mailService;
        }

        #endregion Object and Constructor

        #region Documentation

        /// <summary>
        /// Endpoint para registrar un usuario.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve el usuario registrado.</response>
        /// <response code="400">Errores de validacion.</response>

        #endregion Documentation

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserRegistrationDTO newUser)
        {
            var result = await _userServices.UserExistsByEmail(newUser.Email);
            if (result != null)
                return BadRequest($"El usuario con el email {newUser.Email} ya esta en uso.");

            var registered = await _userServices.RegisterAsync(newUser);
            await _mailService.SendEmailRegisteredUser(registered.Email, $"{registered.FirstName} {registered.LastName}");

            var registerLogin = new UserLoginRequestDTO
            {
                Email = newUser.Email,
                Password = newUser.Password
            };

            var resultLogin = await _userServices.LoginAsync(registerLogin);

            return Ok(resultLogin);
        }

        #region Documentation

        /// <summary>
        /// Endpoint para loguear un usuario.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve el usuario logueado y genera un token unico para el usuario.</response>
        /// <response code="400">Errores de validación.</response>

        #endregion Documentation

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(UserLoginRequestDTO userLogin)
        {
            try
            {
                var result = await _userServices.LoginAsync(userLogin);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        #region Documentation

        /// <summary>
        /// Endpoint para obtener los datos de un usuario por medio de un token.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve el profile del usuario por medio del Token.</response>
        /// <response code="400">Errores de validación.</response>

        #endregion Documentation
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> ProfileAsync()
        {
            try
            {
                var claimsIdentity = this.User.Identity as ClaimsIdentity;
                var userEmail = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value;

                if(userEmail == null)
                    return BadRequest();

                var result = await _userServices.UserExistsByEmail(userEmail);
                if (result == null)
                    return BadRequest();
                    
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}