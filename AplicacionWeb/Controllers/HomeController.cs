using AplicacionWeb.Models;
using BLL.Interfaces;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AplicacionWeb.Controllers
{
    [Authorize] //Esto es para que no pueda ir al HOME sin Iniciar Sesion
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductoService _productoServicio;

        public HomeController(ILogger<HomeController> logger, IProductoService productoServicio)
        {
            _productoServicio = productoServicio;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //OBTENER USUARIO 
            ClaimsPrincipal claimUser = HttpContext.User;
            string nombre = "";
            if (claimUser.Identity.IsAuthenticated)
            {
                nombre = claimUser.Claims.Where(c=> c.Type == ClaimTypes.Name).Select(c=>c.Value).FirstOrDefault();
            }
            //MOSTRAR NOMBRE EL HTML
            ViewData["Mensaje"] = nombre;

            //LISTA PRODUCTOS
            List<Producto> products = await _productoServicio.Lista();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
