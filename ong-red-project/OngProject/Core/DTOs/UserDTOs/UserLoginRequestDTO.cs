﻿using System.ComponentModel.DataAnnotations;

namespace OngProject.Core.DTOs.UserDTOs
{
    public class UserLoginRequestDTO
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(320, ErrorMessage = "El campo {0} debe tener un maximo de {1} caracteres.")]
        [EmailAddress(ErrorMessage = "El campo {0} no es un formato válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(64, ErrorMessage = "El campo {0} debe tener un maximo de {1} caracteres.")]
        public string Password { get; set; }
    }
}