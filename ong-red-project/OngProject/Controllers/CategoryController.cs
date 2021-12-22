using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        #region Object and Constructor
        private readonly ICategoriesServices _CategoriesServices;
        public CategoryController(ICategoriesServices CategoriesServices)
        {
            _CategoriesServices = CategoriesServices;
        }
        #endregion


        [HttpDelete("{id}")]
        public async Task<ActionResult<Result>> Delete(int id)
        {
            var request = await _CategoriesServices.Delete(id);

            return request.HasErrors
                ? BadRequest(request.Messages)
                : Ok(request);
        }

        /// <summary>
        /// Endpoint para obtener los nombres de categorias como administrador
        /// </summary>
        /// <response code="200">Lista de todos los nombres de categoria.</response>
        /// <response code="404">No se encuentran categorias</response>
        /// <response code="401">Credenciales no validas</response> 
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            var categories = await _CategoriesServices.GetCategories();
            if (categories.Length > 0)
            {
                return StatusCode(200, categories);
            }
            return StatusCode(404, new
            {
                Messages = new string[]{
                    String.Format("No se encuentran categorias.}")
                },
                HasErrors = false
            });
        }
    }
}
