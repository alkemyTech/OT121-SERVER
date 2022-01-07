using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs.TestimonialsDTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialsServices _testimonialsServices;

        public TestimonialsController(ITestimonialsServices testimonialsServices)
        {
            _testimonialsServices = testimonialsServices;
        }

        #region Documentation

        /// <summary>
        /// Crear nuevo testimonio en base de datos.
        /// </summary>
        /// <remarks>
        /// Para crear un nuevo testimonio en la base de datos, debe acceder como "Administrator"
        /// </remarks>
        /// <param name="testimonialsCreate">Objeto a crear a la base de datos.</param>
        /// <response code="201">Created. Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>        
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Informa errores de validacion o excepciones.</response>
        #endregion 

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] TestimonialsCreateDTO testimonialsCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Created(nameof(GetAllAsync), await _testimonialsServices.CreateAsync(testimonialsCreate));
                }
                catch (Exception e)
                {
                    return BadRequest(new Result().Fail($"{e.Message}"));
                }
            }

            return BadRequest(new Result().Fail("Algo salio mal."));
        }

        #region Documentation

        /// <summary>
        /// Actualizar un testimonio en la base de datos.
        /// </summary>
        /// <remarks>
        /// Para actualizar un testimonio en la base de datos, debe acceder como "Administrator"
        /// </remarks>
        /// <param name="testimonialsUpdate">Objeto para actualizar la base de datos.</param>
        /// <param name="id">Id del objeto.</param>
        /// <response code="200">OK. Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">BadRequest. No se ha creado el objeto en la BD. Informa errores de validacion o excepciones.</response>

        #endregion 

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromForm] TestimonialsUpdateDTO testimonialsUpdate, int id)
        {
            if (id != testimonialsUpdate.Id)
            {
                return BadRequest(new Result().Fail("Los ids deben coincidir."));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _testimonialsServices.UpdateAsync(testimonialsUpdate));
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
        /// Obtener una pagina de la lista de testimonios
        /// </summary>
        /// <remarks>
        /// Obtiene un listado de 10 testimonios, debe acceder con credenciales validas.
        /// </remarks>
        /// <param name="page">Indica numero de pagina de la lista de testimonios.</param>
        /// <response code="200">OK. Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">BadRequest. Informa que la pagina no existente.</response>
        /// <response code="401">Unauthorized. Credenciales no validas</response> 

        #endregion 

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
        {
            int quantity = 10;
            var pagination = await _testimonialsServices.GetByPagingAsync(page, quantity);
            if (pagination == null)
                return BadRequest(new Result().Fail("La pagina no existente."));
            return Ok(pagination);
        }
    }
}