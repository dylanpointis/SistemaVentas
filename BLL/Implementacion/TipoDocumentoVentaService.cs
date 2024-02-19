using BLL.Interfaces;
using DAL.Interfaces;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementacion
{
    public class TipoDocumentoVentaService : ITipoDocumentoVentaService
    {
        private readonly IGenericRepository<TipoDocumentoVenta> _repositorio;
        public TipoDocumentoVentaService(IGenericRepository<TipoDocumentoVenta> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<List<TipoDocumentoVenta>> Lista()
        {
            IQueryable<TipoDocumentoVenta> lista = await _repositorio.Consultar();
            return lista.ToList();
        }
    }
}
