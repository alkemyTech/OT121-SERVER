using Microsoft.AspNetCore.Authorization;
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
    public class ActivitiesController : ControllerBase
    {
        #region Object and Constructor

        private readonly IActivitiesServices _activitiesServices;

        public ActivitiesController(IActivitiesServices activitiesServices)
        {
            _activitiesServices = activitiesServices;
        }

        #endregion Object and Constructor

        #region Documentacion

        /// <summary>
        /// Endpoint para obtener todas las de Actividades. Se debe ser 'ADMINISTRADOR' para consumir este recurso.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>

        #endregion Documentacion

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IEnumerable<ActivitiesDTO>> GetAll()
        {
            return await _activitiesServices.GetAll();
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para obtener una actividad por 'Id'. Se debe ser 'ADMINISTRADOR' para consumir este recurso
        /// </summary>
        /// <param name="id">Id (int) del objeto.</param>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>
        /// <response code="404">El servidor no pudo encontrar el contenido solicitado.</response>

        #endregion Documentacion

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Get(int id)
        {
            if (!_activitiesServices.EntityExists(id))
            {
                return NotFound();
            }
            var activity = await _activitiesServices.GetById(id);
            return Ok(activity);
        }
    }
}