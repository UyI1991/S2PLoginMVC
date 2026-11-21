using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginMVC.Models
{
    // Representacion de Usuarios en SQL
    public partial class Usuario
    {
        // Llave primaria en SQL
        [Key]
        public int IdUsuario { get; set; }

        // Nombre de usuario
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Nickname { get; set; } = string.Empty;

        // Hash de la contraseña generada
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Contrasena { get; set; } = string.Empty;
    }
}