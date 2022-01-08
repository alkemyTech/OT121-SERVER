using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs.CategoriesDTOs;
using OngProject.Core.Helper.Pagination;
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
        private const int QUANTITY = 10;
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
        /// Obtener una pagina de la lista de categorias como usuario.
        /// </summary>
        /// <remarks>
        /// Obtiene un listado de 10 categorias, debe acceder con credenciales validas.
        /// </remarks>
        /// <param name="page">Indica numero de pagina de la lista de categorias.</param>
        /// <response code="200">OK. Tarea ejecutada con exito devuelve un mensaje satisfactorio.</response>
        /// <response code="400">BadRequest. Informa que la pagina no existente.</response>
        /// <response code="401">Unauthorized. Credenciales no validas</response> 
        [Authorize/*(Roles = "Administrator")*/]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync([FromQuery] int page = 1)
        {
            var categories = await _CategoriesServices.GetByPagingAsync(page, QUANTITY);
            if (categories.Items.Count() == 0)
                return BadRequest(new ResultValue<PaginationDTO<string>>(){Messages = new List<string>(){"La pagina no existente."}, HasErrors = true, StatusCode = 400});  
            return Ok(new ResultValue<PaginationDTO<string>>(){StatusCode = 200, Value = categories});
        }

        /// <summary>
        /// Endpoint para obtener una categoria como administrador
        /// </summary>
        /// <response code="200">Se encontro la categoria deseada</response>
        /// <response code="404">No se encuentra la categoria deseada</response>
        /// <response code="401">Credenciales no validas</response> 
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> Get(int id)
        {
            var category = await _CategoriesServices.Get(id);
            return category != null ? Ok(category) : NotFound();
        }

        /// <summary>
        /// Endpoint para crear una categoria como administrador
        /// </summary>
        /// <response code="201">Categoria creada exitosamente.</response>
        /// <response code="400">Errores de validacion.</response>
        /// <response code="401">Usted no esta autorizado.</response>
        /// <response code="500">Problemas internos del servidor.</response> 
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<ActionResult> Insert([FromForm] CategoryInsertDTO newCategory){
            bool exists = await _CategoriesServices.ExistsByName(newCategory);
            if(exists){
                return StatusCode(400,new Result().Fail("La categoria ya existe."));
            }
            var category = await _CategoriesServices.Insert(newCategory);
            return category != null ? StatusCode(201, category) : StatusCode(500, new Result().Fail("No se logro crear. Problemas del servidor"));
        }

        /// <summary>
        /// Endpoint para editar una categoria como administrador
        /// </summary>
        /// <response code="200">Categoria editada exitosamente.</response>
        /// <response code="404">No se encontro la categoria.</response>
        /// <response code="401">Usted no esta autorizado.</response>
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromForm] CategoryUpdateDTO newCategoryInfo, int id)
        {
            var updatedCategory = await _CategoriesServices.Update(newCategoryInfo, id);
            return updatedCategory != null ? Ok(new Result().Success($"Categoria {id} modificada exitosamente.")) : StatusCode(404, new Result().Fail($"No se encontro la categoria con id {id}."));
        }
    }
}
