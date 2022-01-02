using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class BaseSlideDTO
    {
        [Required]
        [Column(TypeName = "VARCHAR(4000)")]
        [MaxLength(4000)]
        public string Text { get; set; }

        [Column(TypeName = "INTEGER")]
        public virtual int? Order { get; set; }

        [Required]
        public int? OrganizationId { get; set; } = null;

    }
}