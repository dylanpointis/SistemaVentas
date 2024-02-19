using AplicacionWeb.Models;
using BLL.Interfaces;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AplicacionWeb.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
       
        private readonly ICategoriaService _categoriaServicio;
        public CategoriaController(ICategoriaService categoriaServicio)
        {
            _categoriaServicio = categoriaServicio;
        }

        public async Task<IActionResult> Index()
        {
            List<Categoria> listaCat = await _categoriaServicio.Lista();
            return View(listaCat);
        }

        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            CategoriaVM categoriaVM = new CategoriaVM();
            return View(categoriaVM);
        }
        

        [HttpPost]
        public async Task<IActionResult> Crear(CategoriaVM catVM)
        {
            if (ModelState.IsValid)//Verifica si lleno todos los campos
            {
                Categoria categoria = catVM.oCategoria;
                await _categoriaServicio.Crear(categoria);
                return RedirectToAction("Index", "Categoria");
            }
            else
            {
                return View(catVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Editar(int idCategoria)
        {
            CategoriaVM categoriaVM = new CategoriaVM();
            categoriaVM.oCategoria = await _categoriaServicio.Obtener(idCategoria);
            return View(categoriaVM);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(CategoriaVM catVM)
        {
            if (ModelState.IsValid)//Verifica si lleno todos los campos
            {
                Categoria categoria = catVM.oCategoria;
                await _categoriaServicio.Editar(categoria);
                return RedirectToAction("Index", "Categoria");
            }
            else
            {
                return View(catVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Eliminar(int idCategoria)
        {
            CategoriaVM categoriaVM = new CategoriaVM();
            categoriaVM.oCategoria = await _categoriaServicio.Obtener(idCategoria);
            return View(categoriaVM);
        }




        [HttpPost]
        public async Task<IActionResult> Eliminar(CategoriaVM catVM)
        {
            Categoria categoria = catVM.oCategoria;
            await _categoriaServicio.Eliminar((int)categoria.IdCategoria);
            return RedirectToAction("Index", "Categoria");
         
        }
    }   
}
