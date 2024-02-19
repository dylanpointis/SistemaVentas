using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using Entity;
using DAL.Implementacion;
using BLL.Interfaces;
using BLL.Implementacion;
using BLL.CloudinaryService;

namespace IOC
{
    public static class Dependencia
    {
        
        public static void InyectarDependencia(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<DAL.DbventaContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("CadenaSQL"));         
            });
            
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IVentaRepository, VentaRepository>();

            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ITipoDocumentoVentaService, TipoDocumentoVentaService>();
            services.AddScoped<IVentaService, VentaService>();

            services.AddScoped<IPhotoService, PhotoService>();
        }
    }
}
