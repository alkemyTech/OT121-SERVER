using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs;
using OngProject.Core.DTOs.CommentsDTOs;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        #region Objects and Constructor
        private readonly ICommentsServices _commentsServices;
        private readonly IUserServices _userServices;
        private readonly INewsServices _newServices;
        public CommentsController(ICommentsServices commentsServices, IUserServices userServices, INewsServices newServices)
        {
            _commentsServices = commentsServices;
            _userServices = userServices;
            _newServices = newServices;
        }
        #endregion

         
        #region Documentacion
        /// <summary>
        /// Endpoint para eliminacion de baja logica de un Comentario. Se debe ser ADMINISTRADOR o USUARIO
        /// </summary>
        /// <remarks>
        /// <para>
        /// Formato de solicitud: https:// nombreDelServidor /comments ?id=miembroAborrar
        /// </para>
        /// <para>
        /// Ejemplo de solicitud: https://localhost:44353/comments?id=1
        /// </para>
        /// </remarks>
        /// <param name="id">Id del comentario a borrarse, se recibe por solicitud</param>
        /// <returns>
        /// 
        /// </returns>
        /// <response code="200">Se ha eliminado al comentario correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        #endregion

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (_commentsServices.EntityExists(id))
            {
                if (!await _commentsServices.ValidateCreatorOrAdmin(User, id)) return Forbid();
                return Ok(await _commentsServices.Delete(id));

            }
            return Ok(await _commentsServices.Delete(id));
        }

        #region Documentacion
        /// <summary>
        /// Endpoint para crear un Comentario.
        /// </summary>
        /// <response code="200">Se ha creado el comentario correctamente</response>
        /// <response code="401">Credenciales invalidas</response> 
        /// <response code="404">No se ha encontrado el dato proporcionado.</response>
        #endregion

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateRequestDTO comments)
        {
            var user = await  _userServices.UserExistsById(comments.User_id);
            if (!user)
                return NotFound("No existe el usuario proporcionado.");

            var news = await _newServices.NewsExistsById(comments.News_id);
            if (!news)
                return NotFound("No existe el post/news proporcionado."); 

            var result = await _commentsServices.CreateAsync(comments);

            return Ok(result);
        }
    }
}
