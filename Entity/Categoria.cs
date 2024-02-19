using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Entity;

public partial class Categoria
{
    [AllowNull]
    public int? IdCategoria { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public string? Descripcion { get; set; }

    [AllowNull]
    public bool EsActivo { get; set; }

    [AllowNull]
    public DateTime? FechaRegistro { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
