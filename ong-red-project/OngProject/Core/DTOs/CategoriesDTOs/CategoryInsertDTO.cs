using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs.CategoriesDTOs
{
    public class CategoryInsertDTO
    {
        [Required(ErrorMessage = "El campo nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo descripcion es requerido.")]
        public string Description { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile Image { get; set; }
    }
}