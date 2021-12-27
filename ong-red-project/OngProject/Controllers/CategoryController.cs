using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OngProject.Common;
using OngProject.Core.DTOs.CategoriesDTOs;
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
