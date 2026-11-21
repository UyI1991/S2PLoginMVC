using System.ComponentModel.DataAnnotations;

namespace LoginMVC.Models
{
    // Formulario de actualización de contraseña
    public class ChangePasswordViewModel
    {
        // Se solicita la contraseña actual
        [Required(ErrorMessage = "Ingresa tu contraseña actual.")]
        public string ContrasenaActual { get; set; } = string.Empty;

        // Se solicita la nueva contraseña
        [Required(ErrorMessage = "Ingresa la nueva contraseña.")]
        [MinLength(8, ErrorMessage = "La nueva contraseña debe tener al menos 8 caracteres.")]
        public string NuevaContrasena { get; set; } = string.Empty;

        // Se requiere ingresar nuevamente la contraseña
        [Required(ErrorMessage = "Confirma la nueva contraseña.")]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas nuevas no coinciden.")]
        public string ConfirmarNuevaContrasena { get; set; } = string.Empty;
    }
}