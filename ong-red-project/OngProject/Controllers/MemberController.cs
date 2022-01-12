using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        #region Objects and Constructor

        private readonly IMemberServices _memberServices;

        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }

        #endregion Objects and Constructor

        #region Documentation
        /// <summary>
        /// Endpoint para listar los miembros. Siendo Usuario o Administrador.
        /// </summary>
        /// <remarks>
        /// Lista todos los miembros de la BD siendo Administrador. Ejemplo de URL https://api.example.com/v1/Members/
        /// <br></br>
        /// Lista los miembros de la BD por página (Id) específicada mediante un query-param enviada por el cliente siendo Usuario. Ejemplo de URL https://api.example.com/v1/Members?page=1
        /// </remarks>
        /// <response code="200">Ok. Tarea ejecutada con exito devuelve la lista de miembros.</response>
        /// <response code="400">BadRequest. Errores de validacion o excepciones.</response> 
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el JWT de acceso.</response> 
        /// <response code="403">Forbidden. Usted no posee permisos sobre este recurso..</response> 
        #endregion Documentation

        [Authorize]
        [ProducesResponseType(typeof(ResultValue<>), 200)]
        [ProducesResponseType(typeof(ResultValue<>), 400)]
        [ProducesResponseType(typeof(Result), 401)]
        [ProducesResponseType(typeof(ResultValue<>), 403)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery][Range(1, int.MaxValue, ErrorMessage = "No se admiten números negativos o igual a 0")] int? page)
        {

            if (User.IsInRole("Administrator") && !page.HasValue)
            {
                var resultAdmin = await _memberServices.GetAllAsync();
                return StatusCode(resultAdmin.StatusCode, resultAdmin);
            }
            if (!page.HasValue && User.IsInRole("Standard"))
                return StatusCode(403, new ResultValue<IActionResult>() { StatusCode = 403, HasErrors = true, Messages = new List<string>() { "Usted no posee permisos sobre este recurso." } });
            
            var result = await _memberServices.GetAllByPaginationAsync((int)page);
                return StatusCode(result.StatusCode, result);
        }

        #region Documentation
        /// <summary>
        /// Endpoint para crear un miembro nuevo.
        /// </summary>
        /// <remarks>
        /// Crea un nuevo miembro en la BD mediante un formulario. Ejemplo de URL https://api.example.com/v1/Members/
        /// </remarks>
        /// <response code="200">Ok. Tarea ejecutada con exito, crea un miembro nuevo.</response>
        /// <response code="400">BadRequest. Errores de validacion o excepciones.</response> 
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el JWT de acceso.</response> 
        #endregion Documentation

        [Authorize]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 401)]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] MemberInsertDTO member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _memberServices.CreateAsync(member));
                }
                catch (Exception ex)
                {
                    return BadRequest(new Result().Fail($"Ocurrio un error al momento de intentar ingresar los datos - {ex.Message}"));
                }
            }

            return BadRequest(new Result().Fail("Algo salio mal."));
        }

        #region Documentation

        /// <summary>
        /// Endpoint para actualizar un miembro existente.
        /// </summary>
        /// <remarks>
        /// Actualiza un miembro existente con el Id proporcionado en la BD mediante un formulario. Ejemplo de URL https://api.example.com/v1/Members/Id
        /// </remarks>
        /// <response code="200">Ok. Tarea ejecutada con exito, actualiza un miembro existente.</response>
        /// <response code="400">BadRequest. Errores de validacion o excepciones.</response> 
        /// <response code="401">Unauthorized. No se ha indicado o es incorrecto el JWT de acceso.</response> 
        #endregion Documentation

        [Authorize]
        [ProducesResponseType(typeof(Result), 200)]
        [ProducesResponseType(typeof(Result), 400)]
        [ProducesResponseType(typeof(Result), 401)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] MemberUpdateDTO memberUpdate, int id)
        {
            if (id != memberUpdate.Id)
            {
                return BadRequest(new Result().Fail("Los ids deben coincidir."));
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _memberServices.UpdateAsync(memberUpdate));
                }
                catch (Exception ex)
                {
                    return BadRequest(new Result().Fail($"Ocurrio un error al momento de intentar actulizar los datos - {ex.Message}"));
                }
            }

            return BadRequest(new Result().Fail("Algo salio mal."));
        }
    }
}
