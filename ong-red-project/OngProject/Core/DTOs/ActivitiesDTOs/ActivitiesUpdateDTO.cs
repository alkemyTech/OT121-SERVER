﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.ActivitiesDTOs
{
    public class ActivitiesUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}