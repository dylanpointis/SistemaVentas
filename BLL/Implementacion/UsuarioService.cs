using BLL.Interfaces;
using DAL.Implementacion;
using DAL.Interfaces;
using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Implementacion
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IGenericRepository<Usuario> _repositorio;

        public UsuarioService(IGenericRepository<Usuario> repositorio)
        {
            _repositorio = repositorio;
        }


        public string EncriptarClave(string clave)
        {
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] resultado = hash.ComputeHash(enc.GetBytes(clave));
                
                StringBuilder sb = new StringBuilder(); 
                foreach(byte b in resultado)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public async Task<Usuario> ObtenerUsuario(string correo, string clave)
        {
            IQueryable<Usuario> query  =  await _repositorio.Consultar(u => u.Correo == correo && u.Clave == clave);

            Usuario user = query.Include(r => r.IdRolNavigation).FirstOrDefault(); //Hay que incluir el ROL
            return user;
        }

        public async Task<Usuario> RegistrarUsuario(Usuario user)
        {
            return await _repositorio.Crear(user);
        }
    }
}
