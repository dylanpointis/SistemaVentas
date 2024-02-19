using BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using Entity;

namespace BLL.Implementacion
{
    public class ProductoService : IProductoService
    {
        private readonly IGenericRepository<Producto> _repositorio;
        public ProductoService(IGenericRepository<Producto> repositorio)
        {
            _repositorio = repositorio;
        }
        public async Task<List<Producto>> Lista()
        {
            IQueryable<Producto> query = await _repositorio.Consultar();
            return await query.Include(c=> c.IdCategoriaNavigation).ToListAsync();
        }

        public async Task<Producto> Crear(Producto entidad)
        {
            try
            {
                Producto productoCreado = await _repositorio.Crear(entidad);
                if(productoCreado.IdProducto == 0) 
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }
                //Consulto el producto para incluir la Categoria
                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == productoCreado.IdProducto);
                productoCreado = query.Include(c => c.IdCategoriaNavigation).First();
                return productoCreado;
            }
            catch { throw; }
        }

        public async Task<Producto> Editar(Producto entidad)
        {
            try
            {
                //Compruebo que no haya ingreasdo un codigo de barra que ya existe en otro producto (Mismo cod barra, distinto idProd)
                Producto producto_existe = await _repositorio.Obtener(p => p.CodigoBarra == entidad.CodigoBarra && p.IdProducto != entidad.IdProducto);
                if (producto_existe != null) { throw new TaskCanceledException("El codigo de barra ya esta asignado a otro producto"); }


                bool respuesta = await _repositorio.Editar(entidad);
                if (respuesta == false) { throw new TaskCanceledException("No se pudo editar el producto"); }

                //Consulto el producto para incluir la Categoria
                IQueryable<Producto> query = await _repositorio.Consultar(p => p.IdProducto == entidad.IdProducto);
                entidad = query.Include(c => c.IdCategoriaNavigation).First();
                return entidad;

            }
            catch { throw; }
        }

        public async Task<bool> Eliminar(int idProducto)
        {
            try
            {
                Producto prodAEliminar = await _repositorio.Obtener(p => p.IdProducto == idProducto);
                if(prodAEliminar == null) { throw new TaskCanceledException("No se encontró al producto"); }
                bool respuesta =await _repositorio.Eliminar(prodAEliminar);

                return respuesta;
            }
            catch { throw; }
        }

        public async Task<Producto> Obtener(int idProducto)
        {
            Producto prod = await _repositorio.Obtener(p => p.IdProducto == idProducto);
            if (prod == null) { throw new TaskCanceledException("No se encontró a el producto"); }
            return prod;
        }



        public async Task<List<Producto>> BuscarProductos(string busqueda) //Lo mismo que el metodo ObtenerProductos de Venta pero sin EsActivo y Stock > 0
        {
            IQueryable<Producto> query = await _repositorio.Consultar(
                p => string.Concat(p.CodigoBarra, p.Marca, p.Nombre, p.Descripcion).Contains(busqueda) 
                );                                                                       
            return query.Include(c => c.IdCategoriaNavigation).ToList();
        }

    }
}
