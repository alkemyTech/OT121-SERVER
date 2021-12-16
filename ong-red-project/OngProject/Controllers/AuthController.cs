using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.UserDTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Object and Constructor

        private readonly IUserServices _userServices;

        public AuthController(IUserServices _userServices)
        {
            this._userServices = _userServices;
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
            return Ok(await _userServices.RegisterAsync(newUser));
        }

        #region Documentation

        /// <summary>
        /// Endpoint para loguear un usuario.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve el usuario logueado.</response>
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
    }
}