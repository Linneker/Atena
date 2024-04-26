using Microsoft.AspNetCore.Mvc;

namespace acme.sistemas.atena.mvc.site.Controllers.Inventory
{
    public class EstoqueController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
