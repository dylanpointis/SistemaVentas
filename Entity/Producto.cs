using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Entity;

public partial class Producto
{

    public int? IdProducto { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public string CodigoBarra { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public string Marca { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public string Descripcion { get; set; }

    [AllowNull]
    public int? IdCategoria { get; set; }
    
    [AllowNull]
    public int? Stock { get; set; }

    [NotMapped] //HAY QUE PONER ESTO SI NO NO FUNCIONA LA IMAGEN
    public IFormFile? Imagen { get; set; }

    [AllowNull]
    public string? UrlImagen { get; set; }

    [AllowNull]
    public string? NombreImagen { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public decimal Precio { get; set; }

    [AllowNull]
    public bool? EsActivo { get; set; }

    [AllowNull]
    public DateTime? FechaRegistro { get; set; }

    [AllowNull]
    public virtual Categoria? IdCategoriaNavigation { get; set; }

}
