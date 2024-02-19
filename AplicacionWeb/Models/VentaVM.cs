using Entity;

namespace AplicacionWeb.Models
{
    public class VentaVM
    {
        public int IdTipoDocumentoVenta { get; set; }
        public string DocumentoCliente { get; set; }
        public string NombreCliente { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ImpuestoTotal { get; set; }
        public decimal Total { get; set; }
        public ICollection<DetalleVentaVM> DetalleVenta { get; set; }
    }
}
