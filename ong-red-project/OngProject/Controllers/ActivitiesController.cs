using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.ActivitiesDTOs;
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
        #endregion
        #region Documentacion
        /// <summary>
        /// Endpoint para obtener todas las entradas de Activities. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [HttpGet]
        public async Task<IEnumerable<ActivitiesDTO>> GetAll()
        {
            return await _activitiesServices.GetAll();
        }
        #region Documentacion
        /// <summary>
        /// Endpoint para obtener una activity por id. Se debe ser ADMINISTRADOR
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="401">Credenciales no validas</response> 
        #endregion
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if(!_activitiesServices.EntityExists(id))
            {
                return NotFound();
            }
            var activity = await _activitiesServices.GetById(id);
            return Ok(activity);
        }


        #region Documentation

        /// <summary>
        /// Endpoint para actualizar una Activities.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>
        /// <response code="401">Credenciales invalidas</response>  

        #endregion Documentation

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdatePutAsync(int id, [FromForm] ActivitiesUpdateDTO activitiesUpdateDto)
        {
            if (id != activitiesUpdateDto.Id)
                return BadRequest(new Result().Fail("Los Ids deben ser iguales."));
            var update = await _activitiesServices.UpdatePutAsync(activitiesUpdateDto);
            if (update != null)
                return Ok(update);
            return BadRequest(new Result().Fail("El usuario no existe o se produjo un error."));
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para crear Actividades
        /// </summary>
        /// <response code="201">Solicitud concretada con exito</response>
        /// <response code="400">Errores de validacion.</response>
        /// <response code="401">Credenciales invalidas</response>  
        #endregion

        [HttpPost()]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> CreateAsync([FromForm] ActivitiesCreateDTO activitiesDTO)
        {
            var activities = await _activitiesServices.CreateAsync(activitiesDTO);
            return Created(nameof(Get), new { Id = activities.Id });
        }

    }
}
