using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Interfaces;
using Entity;
using Microsoft.EntityFrameworkCore;



namespace BLL.Implementacion
{
    public class VentaService : IVentaService
    {
        private readonly IGenericRepository<Producto> _productoRepositorio; // repositorio generico
        private readonly IVentaRepository _ventaRepositorio;  //repositorio especifico para las ventas

        public VentaService(IGenericRepository<Producto> productoRepositorio, IVentaRepository ventaRepositorio)
        {
            _productoRepositorio = productoRepositorio;
            _ventaRepositorio = ventaRepositorio;
        }

        public async Task<List<Producto>> ObtenerProductos(string busqueda) //metodo para buscar productos y agregarlo a la venta
        {
            IQueryable<Producto> query = await _productoRepositorio.Consultar(
                p => p.EsActivo == true
                && p.Stock > 0 
                && string.Concat(p.CodigoBarra, p.Marca, p.Descripcion).Contains(busqueda) //Esto es para buscar productos segun Codigo barra, marca o descripcion en el formulario de VENTA. 
                );                                                                         //Concatena todos las columnas en un string y luego busca con el metodo Contain las letras que coincidad con el filtro Busqueda recibido por parametro
            return query.Include(c=> c.IdCategoriaNavigation).ToList();
        }

        public Task<Venta> Registrar(Venta venta)
        {
            venta.FechaRegistro = DateTime.Now.Date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);
            try
            {
                return _ventaRepositorio.Registrar(venta);
            }
            catch { throw; }
        }



        public async Task<List<Venta>> Historial(string numVenta, string fechaInicio, string fechaFin)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            fechaInicio = fechaInicio is null ? "": fechaInicio; //SI LA FECHA DE INICIO ES NULL SE HACE VACIO "", SI NO QUEDA IGUAL.
            fechaFin = fechaFin is null ? "" : fechaFin; //Lo mismo

            if(fechaInicio != "" && fechaFin != "") //LOGICA PARA BUSCAR VENTAS ENTRE FECHA INICIO Y FIN
            {
                //CONVERTIR LOS STRING FECHA EN DATETIME
                DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "yyyy-MM-dd", new CultureInfo("es-AR")); 
                DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "yyyy-MM-dd", new CultureInfo("es-AR"));

                //FILTRA EL QUERY ENTRE LA FECHA INICIO Y FIN
                return await query.Where(
                    v => v.FechaRegistro.Value.Date >= fecha_Inicio.Date &&
                    v.FechaRegistro.Value.Date <= fecha_Fin.Date
                )
                .Include(tdv => tdv.IdTipoDocumentoVentaNavigation) //agregar los include en el query
                .Include(u => u.IdUsuarioNavigation)
                .Include(dv => dv.DetalleVenta).ToListAsync();
            }
            else //LOGICA PARA BUSCAR POR NUM DE VENTA. SI NO QUIERO USAR UN RANGO DE FECHAS
            {
                return await query.Where( v => v.NumeroVenta == numVenta)
                .Include(tdv => tdv.IdTipoDocumentoVentaNavigation) //agregar los include en el query
                .Include(u => u.IdUsuarioNavigation)
                .Include(dv => dv.DetalleVenta).ToListAsync();
            }
        }


        public async Task<Venta> Detalle(string numVenta)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar(v => v.NumeroVenta == numVenta);
            
            return query.Include(tdv => tdv.IdTipoDocumentoVentaNavigation) //agregar los include en el query
               .Include(u => u.IdUsuarioNavigation)
               .Include(dv => dv.DetalleVenta)
               .First();
        }


        public async Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin)
        {
            //CONVERTIR LOS STRING FECHA EN DATETIME
            DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-AR"));
            DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-AR"));


            List<DetalleVenta> lista = await _ventaRepositorio.Reporte(fecha_Inicio, fecha_Fin);
            return lista;
        }
    }
}
