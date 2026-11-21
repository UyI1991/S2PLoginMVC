using System.ComponentModel.DataAnnotations;

namespace LoginMVC.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Ingresa tu contraseña actual.")]
        public string ContrasenaActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa la nueva contraseña.")]
        [MinLength(8, ErrorMessage = "La nueva contraseña debe tener al menos 8 caracteres.")]
        public string NuevaContrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma la nueva contraseña.")]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas nuevas no coinciden.")]
        public string ConfirmarNuevaContrasena { get; set; } = string.Empty;
    }
}