using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.NewsDTOs;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsServices _newsServices;
        public NewsController(INewsServices newsServices)
        {
            _newsServices = newsServices;
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para listar los comentarios pertenecientes a un post. 
        /// </summary>
        /// <response code="200">Listado de todos los comentarios pertenecientes a un post .</response>
        /// <response code="401">Credenciales invalidas.</response> 
        /// <response code="404">No se ha encontrado el dato proporcionado.</response>
        #endregion
        [Authorize]
        [HttpGet("{id}/comments")]
        public async Task<ActionResult> GetAllCommentsByNews(int id)
        {
            var result = await _newsServices.GetAllCommentsByNews(id);
            if (result == null)
                return NotFound("No existe el post");
            return Ok(result);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener todos los datos de News por Id
        /// </summary>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="404">No encontrado</response> 
        #endregion
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var news = await _newsServices.GetAllDataByIdAsync(id);
            if (news == null)
                return NotFound();
            return Ok(news);
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para crear Novedades
        /// </summary>
        /// <response code="201">Solicitud concretada con exito</response>
        /// <response code="400">Errores de validacion.</response>
        /// <response code="401">Credenciales invalidas</response>  
        #endregion

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateAsync(NewsDTO newsDTO)
        {
            var news = await _newsServices.CreateAsync(newsDTO);
            return Created(nameof(GetAsync), new { Id = news.Id });
        }
        #region Documentation

        /// <summary>
        /// Endpoint para actualizar una Novedad.
        /// </summary>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>
        /// <response code="401">Credenciales invalidas</response>  

        #endregion Documentation

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePutAsync(int id, [FromForm] NewsUpdateDTO newsUpdateDto)
        {
            if (id != newsUpdateDto.Id)
                return BadRequest(new Result().Fail("Los Ids deben ser iguales."));
            var update = await _newsServices.UpdatePutAsync(newsUpdateDto);
            if (update != null)
                return Ok(newsUpdateDto);
            return BadRequest(new Result().Fail("El usuario no existe o se produjo un error."));
        }
    }
}
