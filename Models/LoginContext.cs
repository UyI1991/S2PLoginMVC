using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginMVC.Models
{
    // Captura y valida los datos de Inicio de sesión
    public class LoginViewModel
    {

    // Solicitud de usuario
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string Nickname { get; set; } = string.Empty;

    // Solicitud de contraseña
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Contrasena { get; set; } = string.Empty;
    }
}

namespace LoginMVC.Models
{
    // Conexión en Entity Framework y SQL
    public class ApplicationDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuración
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        // Representación de la tabla Usuarios en SQL
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
