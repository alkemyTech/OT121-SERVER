using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.ActivitiesDTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
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
        /// <response code="200">Tarea ejecutada con exito devuelve una lista de actividades.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>

        #endregion Documentacion

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ActivitiesDTO), 200)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(Result), 403)]
        public async Task<IEnumerable<ActivitiesDTO>> GetAll()
        {
            return await _activitiesServices.GetAll();
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para obtener una actividad por 'Id'. Se debe ser 'ADMINISTRADOR' para consumir este recurso.
        /// </summary>
        /// <param name="id">Id (int) del objeto.</param>
        /// <response code="200">Tarea ejecutada con exito devuelve la actividad requerida.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>
        /// <response code="404">El servidor no pudo encontrar el contenido solicitado.</response>

        #endregion Documentacion

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(ActivitiesDTO), 200)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(Result), 403)]
        [ProducesResponseType(typeof(Result), 404)]
        public async Task<IActionResult> Get(int id)
        {
            if (!_activitiesServices.EntityExists(id))
            {
                return NotFound(new Result().Fail($"El registro {id} no fue encontrado."));
            }
            var activity = await _activitiesServices.GetById(id);
            return Ok(activity);
        }

        #region Documentation

        /// <summary>
        /// Endpoint para actualizar una actividad por 'Id'. Se debe ser 'ADMINISTRADOR' para consumir este recurso.
        /// </summary>
        /// <param name="activitiesUpdateDto">Objeto a actualizar.</param>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>

        #endregion Documentation

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(Result), 403)]
        public async Task<IActionResult> UpdatePutAsync(int id, [FromForm] ActivitiesUpdateDTO activitiesUpdateDto)
        {
            if (id != activitiesUpdateDto.Id)
                return BadRequest(new Result().Fail("Los Ids deben ser iguales."));

            if (ModelState.IsValid)
            {
                try
                {
                    var update = await _activitiesServices.UpdatePutAsync(activitiesUpdateDto);

                    if (update == null)
                        return BadRequest(new Result().Fail($"Ocurrio un problema al intentar actualizar la actividad. La actividad con el id '{activitiesUpdateDto.Id}' no existe o se produjo un error."));

                    return Ok(new Result().Success($"El registro con el id '{activitiesUpdateDto.Id}' fue actualizado exitosamente."));
                }
                catch (Exception e)
                {
                    return BadRequest(new Result().Fail(e.Message));
                }
            }

            return BadRequest(new Result().Fail("Algo salio mal."));
        }

        #region Documentacion

        /// <summary>
        /// Endpoint para crear una actividad. Se debe ser 'ADMINISTRADOR' para consumir este recurso.
        /// </summary>
        /// <param name="activitiesDTO">Objeto a crear.</param>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>
        /// <response code="401">Se requieren persmisos para acceder al contenido. Debe estar autenticado en la aplicación.</response>
        /// <response code="403">No posee los permisos necesarios para acceder al contenido.</response>

        #endregion Documentacion

        [HttpPost()]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(Result), 403)]
        public async Task<IActionResult> CreateAsync([FromForm] ActivitiesCreateDTO activitiesDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _activitiesServices.CreateAsync(activitiesDTO);
                    return Ok(new Result().Success("Datos guardados satisfactoriamente."));
                }
                catch (Exception e)
                {
                    return BadRequest(new Result().Fail($"{e.Message}"));
                }
            }
            return BadRequest(new Result().Fail("Algo salio mal."));
        }
    }
}