using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> ObtenerUsuario(string correo, string clave);

        Task<Usuario> RegistrarUsuario(Usuario user);

        string EncriptarClave(string clave);
    }
}
