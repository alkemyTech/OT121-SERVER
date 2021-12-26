using Microsoft.AspNetCore.Authorization;
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
        /// Endpoint para crear un testimoniio.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>

        #endregion Documentation

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TestimonialsCreateDTO testimonialsCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _testimonialsServices.CreateAsync(testimonialsCreate));
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
        /// Endpoint para actualizar un testimoniio.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>

        #endregion Documentation

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
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
    }
}