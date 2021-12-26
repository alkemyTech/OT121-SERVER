using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region Objects and Constructor
        public readonly IUserServices _userServices;
        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }
        #endregion

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener un listado de todos los Usuarios
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [HttpGet()]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _userServices.GetUsersAllDataAsync());
        }
    }
}