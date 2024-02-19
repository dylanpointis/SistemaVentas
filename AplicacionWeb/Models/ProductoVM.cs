using Entity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AplicacionWeb.Models
{
    public class ProductoVM
    {
        public Producto oProducto { get; set; }


        [BindNever]
        public List<SelectListItem> oListaCategoria { get; set; } = new List<SelectListItem>(); //NO SE PORQUE HAY QUE PONER GET SET SI O SI
    }
}
