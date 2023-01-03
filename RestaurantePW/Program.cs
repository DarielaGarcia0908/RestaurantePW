using RestaurantePW.Models;
using RestaurantePW.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    options =>
    {
        options.LoginPath = "/Home/IniciarSesion";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });
// Para poder agregar un servicio propio (nuestro) tenemos las siguientes 3 maneras
//Singleton - Creado unicamente una vez y durara hasta que se reinicie
//Transcient - Crea uno por cada usuario conectado y una vez que se desconecten se destruye 
//Scoped - No recuerdo
builder.Services.AddTransient<MenuService>();
builder.Services.AddDbContext<supertacoContext>(
    optionsBuilder => optionsBuilder.UseMySql("server=localhost;password=root;user=root;database=supertaco", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.27-mysql"))
    );
builder.Services.AddMvc();
var app = builder.Build();

app.UseFileServer();
app.UseRouting();
app.UseAuthentication();//Esto tambien debe de estar antes de los endpoints
app.UseAuthorization();//Esto siempre debe de estar antes de los endpoints
app.UseEndpoints(x =>
{
    x.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
    x.MapDefaultControllerRoute();
});

app.Run();
