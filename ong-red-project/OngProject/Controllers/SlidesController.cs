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
       
    }
}