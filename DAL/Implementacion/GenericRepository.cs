using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Entity;

namespace DAL.Implementacion
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbventaContext _dbContext;

        public GenericRepository(DbventaContext dbContext)
        {
            _dbContext= dbContext;
        }

        public async Task<TEntity> Obtener(Expression<Func<TEntity, bool>> filtro)
        {
            try
            {
                TEntity entidad = await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filtro); //TIENE QUE TENER NoTracking() para hacer los metodos de Editar en los controllers
                return entidad;
            }
            catch{ throw; }
        }

        public async Task<TEntity> Crear(TEntity entidad)
        {
            try
            {
                _dbContext.Set<TEntity>().Add(entidad);
                await _dbContext.SaveChangesAsync();
                return entidad;
            }
            catch { throw; }
        }

        public async Task<bool> Editar(TEntity entidad)
        {
            try
            {
                _dbContext.Update(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }

        public async Task<bool> Eliminar(TEntity entidad)
        {
            try
            {
                _dbContext.Remove(entidad);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch { throw; }
        }

        public async Task<IQueryable<TEntity>> Consultar(Expression<Func<TEntity, bool>> filtro = null) //puede recibir un filtro o no
        {
            //si es filtro es null trae toda la lista. Si hay un filtro usa el Where() para buscar segun el filtro
            IQueryable<TEntity> queryEntidad = filtro == null ? _dbContext.Set<TEntity>(): _dbContext.Set<TEntity>().Where(filtro);
            return queryEntidad;
        }

    }
}
