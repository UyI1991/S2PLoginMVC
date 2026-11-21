using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginMVC.Models
{
    //[Table("Usuarios")]
    public class LoginViewModel
    {
    //[Key]
    //public int IdUsuario { get; set; }

    //public string Nombre { get; set; } = string.Empty;
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public string Nickname { get; set; } = string.Empty;

    // Contiene el hash BCrypt, nunca la contraseña original.
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public string Contrasena { get; set; } = string.Empty;
    }
}

namespace LoginMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
