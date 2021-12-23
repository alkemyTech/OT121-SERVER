using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs.TestimonialsDTOs
{
    public class TestimonialsCreateDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(255, ErrorMessage = "El campo {0} debe tener un maximo de {1} caracteres.")]
        public string Name { get; set; }

        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(8000, ErrorMessage = "El campo {0} debe tener un maximo de {1} caracteres.")]
        public string Content { get; set; }
    }
}