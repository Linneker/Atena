using Microsoft.AspNetCore.Mvc;

namespace acme.sistemas.atena.mvc.site.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
