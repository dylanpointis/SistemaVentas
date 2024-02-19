using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace BLL.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> Lista();
        Task<Producto> Crear(Producto entidad);

        Task<Producto> Editar(Producto entidad);

        Task<bool> Eliminar(int idProducto);
        Task<Producto> Obtener(int idProducto);
        Task<List<Producto>> BuscarProductos(string busqueda);
    }
}
