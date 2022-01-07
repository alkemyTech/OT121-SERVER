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
        /// Registra un usuario.
        /// </summary>
        /// <remarks>
        /// Registra un nuevo usuario en la base de datos.
        /// </remarks>
        /// <param name="newUser">Objeto a crear en la base de datos.</param>
        /// <response code="200">Ok. Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>        
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Informa errores de validacion o excepciones.</response>
        #endregion Documentation

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// Acceso de usuarios.
        /// </summary>
        /// <remarks>
        /// Permite acceso a usuarios registrados en la base de datos.
        /// </remarks>
        /// <param name="userLogin">Objeto con Email y contraseña.</param>
        /// <response code="200">Ok. Tarea ejecutada con exito. Devuelve datos del usuario y su Token.</response>        
        /// <response code="400">BadRequest. Informa errores de validacion o excepciones.</response>

        #endregion Documentation

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <response code="200">OK. Tarea ejecutada con exito devuelve el profile del usuario por medio del Token.</response>
        /// <response code="400">BadRequest. Informa errores de validacion o excepciones.</response>

        #endregion Documentation
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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