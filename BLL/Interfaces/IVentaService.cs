using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace BLL.Interfaces
{
    public interface IVentaService
    {
        Task<List<Producto>> ObtenerProductos(string busqueda);

        Task<Venta> Registrar(Venta venta);

        Task<List<Venta>> Historial(string numVenta, string fechaInicio, string fechaFin);

        Task<Venta> Detalle(string numVenta);

        Task<List<DetalleVenta>> Reporte(string fechaInicio, string fechaFin);
    }
}
