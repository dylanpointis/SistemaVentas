using DAL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.Interfaces;
using AplicacionWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.CloudinaryService;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Authorization;
using BLL.Implementacion;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AplicacionWeb.Controllers
{
    [Authorize]
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoServicio;
        private readonly ICategoriaService _categoriaServicio;
        private readonly IPhotoService _photoServicio;

        
        public ProductoController(IProductoService productoServicio, ICategoriaService categoriaServicio, IPhotoService photoServicio )
        {
            _productoServicio = productoServicio;
            _categoriaServicio = categoriaServicio;
            _photoServicio = photoServicio;
        }

        public async Task<IActionResult> Index()
        {
            List<Producto> listaProductos = await _productoServicio.Lista();
            return View(listaProductos);
        }

        public async Task<IActionResult> Detalle(int idProducto)
        {
            ProductoVM productoVM = new ProductoVM();
            productoVM.oProducto = await _productoServicio.Obtener(idProducto);
            return View(productoVM);
        }



        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            ProductoVM productoVM = new ProductoVM();

            List<SelectListItem> selectListItems = await TraerListaCategoria();
            productoVM.oListaCategoria = selectListItems;

            return View(productoVM);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(ProductoVM productoVM)
        {
            if (ModelState.IsValid)//Verifica si lleno todos los campos
            {
                Producto producto = productoVM.oProducto;

                if(producto.Imagen != null)
                {
                    var resultadoImagen = await _photoServicio.AgregarFoto(producto.Imagen); //AGREGA FOTO EN CLOUDINARY
                    producto.UrlImagen = resultadoImagen.Url.ToString();
                }
                

                await _productoServicio.Crear(producto);
                return RedirectToAction("Index", "Producto");
            }
            else 
            {
                List<SelectListItem> selectListItems = await TraerListaCategoria();
                productoVM.oListaCategoria = selectListItems;
                return View(productoVM);
            }
        }

                                                                                                
        private async Task<List<SelectListItem>> TraerListaCategoria() //Es selectListItem asi se puede se usar en un comboBox
        {
            List<Categoria> listaCat = await _categoriaServicio.Lista();

            List<SelectListItem> selectListItems = listaCat.Select(cat => new SelectListItem()
            {
                Text = cat.Descripcion,
                Value = cat.IdCategoria.ToString()
            }).ToList();

            return selectListItems;
        }


        [HttpGet]
        public async Task<IActionResult> Editar(int idProducto)
        {
            ProductoVM productoVM = new ProductoVM();
            productoVM.oProducto = await _productoServicio.Obtener(idProducto);

            List<SelectListItem> selectListItems = await TraerListaCategoria();
            productoVM.oListaCategoria = selectListItems;

            return View(productoVM);
        }


        [HttpPost]
        public async Task<IActionResult> Editar(ProductoVM prodVM)
        {
            if (ModelState.IsValid)//Verifica si lleno todos los campos
            {
                Producto productoEditado = prodVM.oProducto; //OBTENGO EL PRODUCTO YA EDITADO DESDE EL VIEWMODEL
                Producto productoViejo = await _productoServicio.Obtener((int)prodVM.oProducto.IdProducto); //OBTENGO EL PRODUCTO ANTES DE EDITAR. EL METODO DE OBTENER TIENE QUE TENER NoTracking()

                if (productoEditado.Imagen == null) //SI NO SUBIO NINGUNA FOTO SE MANTIENE LA QUE YA TENIA
                {
                    productoEditado.UrlImagen = productoViejo.UrlImagen;
                }


                if (productoViejo.UrlImagen != null && productoEditado.Imagen != null)
                {
                    await _photoServicio.BorrarFoto(productoViejo.UrlImagen); //BORRO LA IMAGEN DEL PRODUCTO ANTES DE EDITAR

                }

               
                if (productoEditado.Imagen != null)
                {
                    var resultadoImagen = await _photoServicio.AgregarFoto(productoEditado.Imagen); //AGREGA NUEVA FOTO EN CLOUDINARY
                    productoEditado.UrlImagen = resultadoImagen.Url.ToString();
                }


                await _productoServicio.Editar(productoEditado);
                return RedirectToAction("Index", "Producto");
            }
            else
            {
                List<SelectListItem> selectListItems = await TraerListaCategoria();
                prodVM.oListaCategoria = selectListItems;          
                return View(prodVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int idProducto)
        {
            ProductoVM productoVM = new ProductoVM();
            productoVM.oProducto = await _productoServicio.Obtener(idProducto);
            return View(productoVM);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(ProductoVM prodVM)
        {
            Producto producto = await _productoServicio.Obtener((int)prodVM.oProducto.IdProducto);
          
            if (!string.IsNullOrEmpty(producto.UrlImagen))
            {
                _photoServicio.BorrarFoto(producto.UrlImagen); //borra la foto de Cloudinary
            }

            await _productoServicio.Eliminar((int)producto.IdProducto);

            return RedirectToAction("Index", "Producto");
        }

        //Funcion para buscar productos, es casi lo mismo que ObtenerProductos de ventacontroller pero sin JAVASCRIPT
        //no uso js sino que pongo un <form> en el index.cshtml con el input para escribir el filtro y el boton para ejecutar este metodo
        //que devuelve la vista con la lista Producto pasada como modelo
            [HttpGet]
        public async Task<IActionResult> BuscarProductos(string busqueda) 
        { 
            List<Producto> listaProductos = await _productoServicio.BuscarProductos(busqueda);

            return View("Index", listaProductos); //devuelve index en vez de statuscode
        }
    }
}
