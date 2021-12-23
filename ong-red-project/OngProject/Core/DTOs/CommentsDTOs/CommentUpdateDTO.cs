using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs.CommentsDTOs
{
    public class CommentUpdateDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Column(TypeName = "TEXT")]
        [MaxLength(4000)]
        public string Body { get; set; }

    }
}
