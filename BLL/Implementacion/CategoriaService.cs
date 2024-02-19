using BLL.Interfaces;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace BLL.Implementacion
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IGenericRepository<Categoria> _repositorio;

        public CategoriaService(IGenericRepository<Categoria> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<Categoria>> Lista()
        {
            IQueryable<Categoria> query = await _repositorio.Consultar();
            return query.ToList();
        }

        public async Task<Categoria> Crear(Categoria entidad)
        {
            try
            {
                Categoria catCreada = await _repositorio.Crear(entidad);
                if (catCreada.IdCategoria == 0)
                {
                    throw new TaskCanceledException("No se ha podido crear la categoria");
                }
                return catCreada;
            }
            catch { throw; }
        }

        public async Task<Categoria> Editar(Categoria entidad)
        {
            try
            {

                bool respuesta = await _repositorio.Editar(entidad);
                if (respuesta == false) { throw new TaskCanceledException("No se pudo editar el producto"); }

                return entidad;
            }
            catch { throw; }
        }

        public async Task<bool> Eliminar(int idCategoria)
        {
            try
            {

                Categoria catAEliminar = await _repositorio.Obtener(c => c.IdCategoria == idCategoria);
                if (catAEliminar == null) { throw new TaskCanceledException("No se encontró a la categoria"); }
                bool respuesta = await _repositorio.Eliminar(catAEliminar);

                return respuesta;
            }
            catch { throw; }
        }


        public async Task<Categoria> Obtener(int idCategoria)
        {
            Categoria cat = await _repositorio.Obtener(c => c.IdCategoria == idCategoria);
            if (cat == null) { throw new TaskCanceledException("No se encontró a la categoria"); }
            return cat;
        }
    }
}
