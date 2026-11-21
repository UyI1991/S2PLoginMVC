using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LoginMVC.Controllers
{
    // Se bloquea el acceso no autorizado
    [Authorize]
    // Controlador Principal del Dashboard
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}