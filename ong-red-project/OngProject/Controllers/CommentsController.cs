using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Core.DTOs;
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
        public CommentsController(ICommentsServices commentsServices)
        {
            _commentsServices = commentsServices;
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

       
    }
}
