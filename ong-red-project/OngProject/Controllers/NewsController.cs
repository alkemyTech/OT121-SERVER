using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
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
    }
}
