using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginMVC.Models;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace LoginMVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Mostrar pantalla de Login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // Validar credenciales
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Buscar usuario en SQL Server
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Nickname == modelo.Nickname);

            // Verificar usuario y contraseña
            if (usuario == null ||
                !BCrypt.Net.BCrypt.Verify(
                    modelo.Contrasena, usuario.Contrasena))
            {
                ModelState.AddModelError("",
                    "Usuario o contraseña incorrectos.");

                return View(modelo);
            }

            // Crear identidad del usuario
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nickname),
                new Claim(ClaimTypes.NameIdentifier,
                    usuario.IdUsuario.ToString()),
            };

            // Si el usuario es "admin" se asigna el rol Admin
            if (usuario.Nickname.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var identidad = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identidad);

            // Crear sesión autenticada
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Redirigir al Dashboard
            return RedirectToAction("Index", "Home");
        }

        // Cerrar sesión
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // Registro nuevos usuarios
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Validar que el usuario no exista en SQL Server
            bool existe = await _context.Usuarios.AnyAsync(u => u.Nickname == modelo.Nickname);
            if (existe)
            {
                ModelState.AddModelError("Nickname", "Este usuario ya está registrado. Elige otro.");
                return View(modelo);
            }

            // Crear nuevo usuario y encriptar contraseña con BCrypt
            var nuevoUsuario = new Usuario
            {
                Nickname = modelo.Nickname,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(modelo.Contrasena)
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = "Usuario registrado correctamente. Ya puedes iniciar sesión.";
            return RedirectToAction("Login", "Account");
        }

        // Cambio de contraseña
        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            // Obtener el IdUsuario desde la cookie de sesión
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idClaim, out int idUsuario))
            {
                return RedirectToAction("Login", "Account");
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);
            if (usuario == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Verificar que la contraseña actual coincida con el hash guardado
            if (!BCrypt.Net.BCrypt.Verify(modelo.ContrasenaActual, usuario.Contrasena))
            {
                ModelState.AddModelError("ContrasenaActual", "La contraseña actual es incorrecta.");
                return View(modelo);
            }

            // Generar nuevo hash BCrypt y actualizar en SQL Server
            usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(modelo.NuevaContrasena);
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            TempData["MensajeCambio"] = "Tu contraseña ha sido actualizada exitosamente.";
            return RedirectToAction("Index", "Home");
        }

        // Gestión de usuarios desde Admin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ManageUsers()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                TempData["ErrorAdmin"] = "El usuario no existe.";
                return RedirectToAction("ManageUsers");
            }

            // Evitar que el super admin se elimine a sí mismo
            if (usuario.Nickname.Equals("admin", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorAdmin"] = "No puedes eliminar la cuenta principal de administrador.";
                return RedirectToAction("ManageUsers");
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            TempData["ExitoAdmin"] = $"El usuario '{usuario.Nickname}' fue eliminado correctamente.";
            return RedirectToAction("ManageUsers");
        }
    }
}
