using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class SlideDTOForUpdate : BaseSlideDTO
    {
        public IFormFile Image { get; set; }

        [Required]
        public override int? Order { get; set; }

        
    }
}