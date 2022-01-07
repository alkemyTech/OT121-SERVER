using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.CommentsDTOs;
using OngProject.Core.DTOs.NewsDTOs;
using OngProject.Core.Entities;
using OngProject.Core.Helper.Pagination;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Produces("application/json")]
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
        /// <remarks>
        /// Lista por páginas todos los comentarios asociados a un Id de Novedades. Cada página trae a lo sumo 10 items. La página requerida debe ser un número mayor a 0 y menor o igual a la decena del total de cantidad de comentarios de la novedad.
        ///
        ///Se requiere credenciales de Usuario o Administrador.
        ///Ejemplo de solicitud: 
        ///
        ///     GET /News/2/comments?page=1
        ///
        /// </remarks>
        /// <response code="200">Petición exitosa.</response>
        /// <response code="400">Solicitud incorrecta. Id de novedad o número de página inválido.</response>
        /// <response code="401">Credenciales invalidas.</response> 
        /// <response code="403">Forbidden. Usted no posee permisos sobre este recurso.</response> 
        #endregion
        [Authorize]
        [ProducesResponseType(typeof(ResultValue<>), 200)]
        [ProducesResponseType(typeof(ResultValue<>), 400)]
        [ProducesResponseType(typeof(ResultValue<>), 401)]
        [ProducesResponseType(typeof(ResultValue<>), 403)]
        [HttpGet("{id}/comments")]
        public async Task<ActionResult> GetAllCommentsByNews(int id, int page)
        {            
            ResultValue<PaginationDTO<CommentResponseDTO>> result = await _newsServices.GetAllCommentsUsingPaging(id, page);
            if(result.HasErrors)
                 return StatusCode(result.StatusCode, result.Messages[0]);

            return StatusCode(result.StatusCode, result.Value);
            
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para obtener todos los datos de Novedad.
        /// </summary>
        /// <remarks>
        /// Obtiene todos los datos de una Novedad a partir de su Id. 
        ///Si el Id existe en la base se devuelven sus datos asociados.
        ///
        ///No se requieren permisos.
        ///
        ///Ejemplo de solicitud: 
        ///
        ///     GET /News/1
        ///
        /// </remarks>
        /// <response code="200">Solicitud concretada con exito</response>
        /// <response code="404">No encontrado</response> 
        #endregion

        [ProducesResponseType(typeof(News), 200)]
        [ProducesResponseType(typeof(ActionResult), 404)]
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
        /// <remarks>
        ///Crea una novedad agregándola a la base de datos. Retorna el Id de la nueva novedad.
        ///
        /// Se requieren permisos de Usuario o Administrador
        ///
        ///Ejemplo de solicitud: 
        ///
        ///     POST /News
        ///
        /// </remarks>
        /// <response code="201">Solicitud concretada con exito</response>
        /// <response code="400">Errores de validacion.</response>
        /// <response code="401">Credenciales invalidas</response>  
        #endregion
        [ProducesResponseType(typeof(ActionResult), 201)]
        [ProducesResponseType(typeof(ActionResult), 400)]
        [ProducesResponseType(typeof(ActionResult), 401)]
        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromForm] NewsCreateDTO newsDTO)
        {
            var news = await _newsServices.CreateAsync(newsDTO);
            return Created(nameof(GetAsync), new { Id = news.Id });
        }
        #region Documentation

        /// <summary>
        /// Endpoint para actualizar una Novedad.
        /// </summary>
        /// <remarks>
        ///Actualiza una novedad en la base de datos. En caso de éxito retorna la novedad actualizada.
        ///
        /// Se requieren permisos de Usuario o Administrador
        ///
        ///Ejemplo de solicitud: 
        ///
        ///     PUT /News/1
        ///
        /// </remarks>
        /// <response code="200">Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">Errores de validacion o excepciones.</response>
        /// <response code="401">Credenciales invalidas</response>  
        #endregion Documentation
        [ProducesResponseType(typeof(News), 200)]
        [ProducesResponseType(typeof(ActionResult), 400)]
        [ProducesResponseType(typeof(ActionResult), 401)]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePutAsync(int id, [FromForm] NewsUpdateDTO newsUpdateDto)
        {
            if (id != newsUpdateDto.Id)
                return BadRequest(new Result().Fail("Los Ids deben ser iguales."));
            var update = await _newsServices.UpdatePutAsync(newsUpdateDto);
            if (update != null)
                return Ok(update);
            return BadRequest(new Result().Fail("El usuario no existe o se produjo un error."));
        }
    }
}
