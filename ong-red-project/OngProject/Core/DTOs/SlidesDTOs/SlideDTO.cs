using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OngProject.Core.DTOs.SlidesDTOs
{
    public class SlideDTO
    {
        [Required]
        [Column(TypeName = "VARCHAR(4000)")]
        [MaxLength(4000)]
        public string Text { get; set; }

        [Column(TypeName = "INTEGER")]
        public int? Order { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = new DateTime(); 

        public string Base64Image { get; set; }
        
        [Required]
        public int? OrganizationId { get; set; } = null;

    }
}