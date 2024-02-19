namespace AplicacionWeb.Models
{
    public class DetalleVentaVM
    {
        public int IdProducto { get; set; }
        public string Marca { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
    }
}
