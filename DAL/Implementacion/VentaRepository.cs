using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using Entity;

namespace DAL.Implementacion
{
    public class VentaRepository : GenericRepository<Venta>, IVentaRepository
    {
        private readonly DbventaContext _dbContext;

        public VentaRepository(DbventaContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Venta> Registrar(Venta entidad)
        {
            Venta ventaGenerada = new Venta();
            using (var transaccion = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //Cuando se registra una venta hay que reducir el stock de los productos
                    foreach (DetalleVenta dv in entidad.DetalleVenta)
                    {
                        Producto productoVenta = _dbContext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();
                        productoVenta.Stock -= dv.Cantidad; //reduce stock
                        _dbContext.Productos.Update(productoVenta); //actuliza la Bd
                    }
                    await _dbContext.SaveChangesAsync();


                    await _dbContext.Venta.AddAsync(entidad); //agrega la venta en la BD
                    await _dbContext.SaveChangesAsync();

                    entidad.NumeroVenta = entidad.IdVenta.ToString(); //consigue el ID autoincrement y se lo asigna al num de venta
                    await _dbContext.SaveChangesAsync(); //vuelve a guardar en la bd para el num venta

                    ventaGenerada = entidad;
                    transaccion.Commit();
                }
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                catch (Exception ex) 
                {
                    transaccion.Rollback();
                    throw ex;
                }
            }
            return ventaGenerada;
        }

        public async Task<List<DetalleVenta>> Reporte(DateTime FechaInicio, DateTime FechaFin)
        {
            List<DetalleVenta> listaResumen = await _dbContext.DetalleVenta
                .Include(venta => venta.IdVentaNavigation)
                .ThenInclude(user => user.IdUsuario)
                .Include(venta => venta.IdVentaNavigation)
                .ThenInclude(tdv => tdv.IdTipoDocumentoVentaNavigation)
                .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= FechaInicio.Date &&
                dv.IdVentaNavigation.FechaRegistro.Value.Date <= FechaFin.Date).ToListAsync();

            return listaResumen;
        }
    }
}
