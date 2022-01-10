using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.NewsDTOs
{
    public class NewsUpdateDTO
    {
        /// <summary>
        /// Id de la novedad que se actualiza.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Nuevo nombre de la novedad. 
        /// </summary>
        [Required]
        public string Name { get; set; }
        
        /// <summary>
        /// Nuevo contenido de la novedad.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// Nueva imagen de la novedad. Solo formatos png, jpeg o jpg.
        /// </summary>
        [Required]
        public IFormFile Image { get; set; }
        
        /// <summary>
        /// Nuevo Id de categoría de la novedad.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }
    }
}
