using System;
using System.Collections.Generic;

namespace Entity;

public partial class TipoDocumentoVenta
{
    public int IdTipoDocumentoVenta { get; set; }

    public string? Descripcion { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
