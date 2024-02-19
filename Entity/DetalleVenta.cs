using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Entity;

public partial class DetalleVenta
{
    public int IdDetalleVenta { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public string? MarcaProducto { get; set; }


    public string? NombreProducto { get; set; }

    public string? DescripcionProducto { get; set; }

    public string? CategoriaProducto { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Precio { get; set; }

    public decimal? Total { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual Venta? IdVentaNavigation { get; set; }
}
