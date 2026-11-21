using Microsoft.EntityFrameworkCore;
using LoginMVC.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

// Constructor de la aplicacion Web
var builder = WebApplication.CreateBuilder(args);

// Configurar MVC
builder.Services.AddControllersWithViews();

// Conexion con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexionSQL")
    )
);

// Configurar autenticacion
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";

        //options.AccessDeniedPath = "/Account/Login";

        options.Cookie.HttpOnly = true;

        options.Cookie.SecurePolicy =
            CookieSecurePolicy.SameAsRequest;

        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

        options.SlidingExpiration = true;
    });

// Control de acceso por roles
builder.Services.AddAuthorization();

// Constructor de la instancia Web
var app = builder.Build();

// Configurar el sistema
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// Activar autenticacion
app.UseAuthentication();

// Evalua los permisos y roles
app.UseAuthorization();

// Ruta principal
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// Inicio de servidor Web
app.Run();
