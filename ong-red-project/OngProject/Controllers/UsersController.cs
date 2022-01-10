using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Entities;
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

        #endregion Objects and Constructor

        #region Documentacion

        /// <summary>
        /// Endpoint para obtener todos los usuarios. Se debe ser 'ADMINISTRADOR' para consumir este recurso.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve la lista de usuarios.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>

        #endregion Documentacion

        [HttpGet()]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(Result), 403)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _userServices.GetUsersAllDataAsync());
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para actualizar un usuario. Se debe ser 'ADMINISTRADOR' para consumir este recurso.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>
        /// <response code="404">No se encontro el recurso solicitado.</response>

        #endregion Documentacion

        [HttpPatch("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(Result), 403)]
        [ProducesResponseType(typeof(Result), 404)]
        public async Task<IActionResult> PatchAsync(int id, JsonPatchDocument userPatch)
        {
            var user = await _userServices.UpdatePatchAsync(id, userPatch);
            if (user == null)
                return NotFound(new Result().Fail("El usuario no fue encontrado o el email ya esta en uso."));
            return Ok(new Result().Success("Usuario actualizado exitosamente."));
        }
    }
}