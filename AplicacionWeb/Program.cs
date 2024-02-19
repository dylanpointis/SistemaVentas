using BLL.CloudinaryService;
using IOC;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings")); //ESTO ES PARA EL CLOUDINARY
builder.Services.InyectarDependencia(builder.Configuration); //AGREGA LAS INSTANCIAS CON INYECCION DEPENDENCIA

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) //AGREGAR AUTENTICACION POR COOKIE
    .AddCookie(options => 
    {
        options.LoginPath = "/Inicio/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

builder.Services.AddControllersWithViews(options =>  //ESTO ES PARA BORRAR LA CACHE AL CERRAR SESION
{
    options.Filters.Add(
        new ResponseCacheAttribute { NoStore = true, Location = ResponseCacheLocation.None}
        );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();
