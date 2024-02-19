using AplicacionWeb.Models;
using BLL.Interfaces;
using Entity;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;

namespace AplicacionWeb.Controllers
{
    [Authorize]
    public class VentaController : Controller
    {
        private readonly IVentaService _ventaService;
        private readonly ITipoDocumentoVentaService _tipoDocService;

        public VentaController(IVentaService ventaService, ITipoDocumentoVentaService tipoDocService)
        {
            _tipoDocService = tipoDocService;
            _ventaService = ventaService;
        }


        public IActionResult Index()
        {
            //obtengo el Rol. Solo los administradores pueden hacer ventas
            ClaimsPrincipal claimUser = HttpContext.User;
            string Rol = claimUser.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            if(Rol == "Administrador")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListaTipoDocumentoVenta()
        {
            List<TipoDocumentoVenta> listaTipoDoc = await _tipoDocService.Lista();
            return StatusCode(StatusCodes.Status200OK,listaTipoDoc);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerProductos(string busqueda) //funcion para la barra de busqueda en el form ventas. SE NECESITA USAR JAVASCRIPT
        {                                                                   //use select2 para hacer la barra de busqueda y el archivo Nueva_Venta.js
            List<Producto> listaProductos = await _ventaService.ObtenerProductos(busqueda);

            return StatusCode(StatusCodes.Status200OK, listaProductos);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] VentaVM ventaVM) //SIn el [FROMBODY] No funciona
        {
            List<DetalleVenta> detalleVentaModelo = new List<DetalleVenta>();

            foreach (var detalleVM in ventaVM.DetalleVenta)
            {
                DetalleVenta detalleModelo = new DetalleVenta
                {
                    IdProducto = detalleVM.IdProducto,
                    MarcaProducto = detalleVM.Marca,
                    DescripcionProducto = detalleVM.Descripcion,
                    NombreProducto = detalleVM.NombreProducto,
                    Precio = detalleVM.Precio,
                    Total = detalleVM.Total,
                    Cantidad = detalleVM.Cantidad
                };
                detalleVentaModelo.Add(detalleModelo);
            }

            Venta ventaModelo = new Venta
            {
                IdTipoDocumentoVenta = ventaVM.IdTipoDocumentoVenta,
                DocumentoCliente = ventaVM.DocumentoCliente,
                NombreCliente = ventaVM.NombreCliente,
                SubTotal = ventaVM.SubTotal,
                ImpuestoTotal = ventaVM.ImpuestoTotal,
                Total = ventaVM.Total,
                DetalleVenta = detalleVentaModelo
                //la fecha la puse en el Service con el formato "yyyy-MM-dd HH:mm"
            };

            //obtengo el Id del usuario logeado
            ClaimsPrincipal claimUser = HttpContext.User;
            string idUser = claimUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).Select(c => c.Value).FirstOrDefault();

            ventaModelo.IdUsuario = Convert.ToInt32(idUser); //asigno el id


            Venta ventaCreada = await _ventaService.Registrar(ventaModelo); //registro la venta
            return StatusCode(StatusCodes.Status200OK, ventaCreada);
        }

        public IActionResult HistorialVenta()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Historial( string numeroVenta, string fechaInicio, string fechaFin)
        {
            List<Venta> historial = await _ventaService.Historial(numeroVenta, fechaInicio, fechaFin);
            return StatusCode(StatusCodes.Status200OK, historial);
        }
    }
}
