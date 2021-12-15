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
    [Route("organization")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IOrganizationsServices _organizationsServices;
        public OrganizationsController(IOrganizationsServices organizationsServices)
        {
            _organizationsServices = organizationsServices;
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener datos publicos de una organizacion por id. Se debe ser un USUARIO
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="404">La organizacion no existe</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [HttpGet("public")]
        public async Task<IActionResult> GetPublic([FromQuery] int id)
        {
            if (!_organizationsServices.EntityExists(id))
            {
                return NotFound();
            }
            var activity = await _organizationsServices.GetById(id);
            return Ok(activity);
        }
    }
}
