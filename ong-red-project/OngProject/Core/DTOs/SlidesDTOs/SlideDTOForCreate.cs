using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using OngProject.Core.Helper.CustomValidationsAttributes;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class SlideDTOForCreate : BaseSlideDTO
    {
        [Required]
        [Base64]
        public string Base64Image { get; set; }
    }
}

