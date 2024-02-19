using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using BLL.Interfaces;
using Entity;
using CloudinaryDotNet.Actions;
using AplicacionWeb.Models;

namespace AplicacionWeb.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public InicioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string correo, string clave)  //Hay que poner "name" en el html para obtener los parametros
        {
            Usuario user = await _usuarioService.ObtenerUsuario(correo, _usuarioService.EncriptarClave(clave)); //Busca al usuario
            if(user == null)
            {
                ViewData["Error"] = "No se encontraron coincidencias";  //si no lo encuentra muestra el error
                return View();
            }

            List<Claim> listaClaims = new List<Claim>()  //crea una lista de Claims con los datos del Usuario
            {
                new Claim(ClaimTypes.Name,  user.Nombre ),
                new Claim(ClaimTypes.Email, user.Correo),
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new Claim(ClaimTypes.Role, user.IdRolNavigation.Descripcion)
            };
            //Se almacenan los claims y el esquema de autenticacion
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(listaClaims, CookieAuthenticationDefaults.AuthenticationScheme); 
           
            //propieadades de autenticacion
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = true,
            };

            //Se usa el metodo SingIn para almacenar el usuario en la Cookie y luego poder acceder a el usuario en otras partes del codigo, se necesita un claimsPrincipal
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index","Home");
        }


        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM user) //En el html hay que poner el IdRol = 3
        {
            user.oUsuario.Clave = _usuarioService.EncriptarClave(user.oUsuario.Clave);

            Usuario usuarioCreado = await _usuarioService.RegistrarUsuario(user.oUsuario);

            if (usuarioCreado.IdUsuario > 0) //Si se registro correctamente
            {
                return RedirectToAction("IniciarSesion", "Inicio");
            }
            ViewData["Error"] = "No se pudo crear el usuario";
            return View();
        }

        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("IniciarSesion", "Inicio");
        }

    }
}
