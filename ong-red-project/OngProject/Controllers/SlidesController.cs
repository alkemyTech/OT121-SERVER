using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs.SlidesDTOs;
using OngProject.Core.Interfaces.IServices;
using OngProject.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SlidesController : ControllerBase
    {
        #region Object and Contructor
        private readonly ISlidesServices _slidesServices;

        public SlidesController(ISlidesServices slidesServices)
        {
            _slidesServices = slidesServices;
        }
        #endregion


        #region Documentation
        /// <summary>
        /// Devuelve un listado de slides existentes en un determinado orden o una lista vacÃ­a.
        /// </summary>
        /// <response code="200">En caso de existir se devuelve uno o mas slides y si no existen se devuelve lista vacia</response>
        #endregion Documentation
        [HttpGet]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> ListSlides(){
            List<SlideDataShortResponse> result = await _slidesServices.GetListOfSlides();
            return Ok(result);
        }

        #region Documentation
        /// <summary>
        /// Devuelve slide cuyo id coincide con el argumento. Si no lo encuentra se retorna error.
        /// </summary>
        /// <response code="200">En caso de existir se devuelve el slide</response>
        /// <response code="404">Slide no existente</response>
        #endregion Documentation
        [HttpGet("{id}")]
        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> GetSlide(int id)
        {
            bool exists = _slidesServices.EntityExist(id);
            if(exists){
                SlideDataFullResponse result = await _slidesServices.GetSlideById(id);
                return Ok(result);
            }
            else
            {
                return NotFound();
            }
        }

        #region Documentation
        /// <summary>
        /// Crea nueva slide, devuelve su ID en caso de exito.
        /// </summary>
        /// <response code="201">Solicitud concretada con exito</response>
        /// <response code="400">Errores de validacion.</response>
        /// <response code="403">Credenciales invalidas</response>  
        #endregion
        [HttpPost]
        [Authorize(Roles="Administrator")]
        public async Task<IActionResult> CreateSlide(SlideDTO model){
            if(!ModelState.IsValid){
                return BadRequest();
            }
            return Ok(await _slidesServices.CreateSlideAsync(model));
        }

    }
}