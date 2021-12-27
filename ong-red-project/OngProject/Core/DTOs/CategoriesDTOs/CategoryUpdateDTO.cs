using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs.CategoriesDTOs
{
    public class CategoryUpdateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile Image { get; set; }
    }
}