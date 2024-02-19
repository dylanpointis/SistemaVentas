using Microsoft.AspNetCore.Mvc;

namespace AplicacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
