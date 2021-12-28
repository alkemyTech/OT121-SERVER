using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener un listado de todos los Usuarios
        /// </summary>
        /// <response code="204">Solicitud se ha realizado correctamente</response>
        /// <response code="404">Usuario no encontrado o Email ya existe</response> 
        #endregion
        [HttpPatch("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PatchAsync(int id, JsonPatchDocument userPatch)
        {
            var user = await _userServices.UpdatePatchAsync(id, userPatch);
            if (user == null)
                return NotFound(new Result().Fail("Usuario no encontrado o Email ya existe"));
            return NoContent();
        }
    }
}