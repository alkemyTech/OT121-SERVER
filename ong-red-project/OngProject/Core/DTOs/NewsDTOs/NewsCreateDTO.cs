using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.NewsDTOs
{
    public class NewsCreateDTO
    {
        /// <summary>
        /// Nombre de la novedad
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Contenido de la novedad
        /// </summary>
        [Required]
        public string Content { get; set; }
        
        /// <summary>
        /// Imagen de la novedad. Solo formatos png, jpeg o jpg.
        /// </summary>
        [Required]
        public IFormFile Image { get; set; }

        /// <summary>
        /// Id de categoría de la novedad.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
    }
}
