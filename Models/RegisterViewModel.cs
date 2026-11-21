using System.ComponentModel.DataAnnotations;

namespace LoginMVC.Models
{
    // Formulario para registro de nuevos usuarios
    public class RegisterViewModel
    {
        // Se solicita usuario, máx 20 caracteres
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres.")]
        public string Nickname { get; set; } = string.Empty;

        // Se solicita contraseña, mín 8 caracteres
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Contrasena { get; set; } = string.Empty;

        // Se solicita ingresar de nuevo la contraseña
        [Required(ErrorMessage = "Confirma tu contraseña.")]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; } = string.Empty;
    }
}