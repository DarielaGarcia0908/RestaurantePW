using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RestaurantePW.Models;
using RestaurantePW.Models.ViewModels;
using NuGet.Protocol.Plugins;

namespace RestaurantePW.Controllers
{
    public class HomeController : Controller
    {
        private readonly supertacoContext context;

        public HomeController(supertacoContext context)
        {
            this.context = context;
        }
        [Route("/")]
        [Route("/Principal")]
        [Route("/Home")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("/c/{id}")]
        public IActionResult Categoria(string id)
        {
            var datos = context.Categorias.Include(x => x.Platillos)
            .Where(x => x.Nombre == id)
                .Select(x => new CategoriaViewModel
                {
                    NombreCategoria = x.Nombre,
                    Platillos = x.Platillos.Select(x => new Platillo
                    {
                        Id = x.Id,
                        Precio = x.Precio,
                        Nombre = x.Nombre
                    })
                }).FirstOrDefault();
            return View(datos);
        }
        [Route("/p/{nombre}")]
        public IActionResult Ver(string? nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return RedirectToAction("Index");
            nombre = nombre.Replace("-", " ");
            var platillo = context.Platillos.Include(x => x.IdCategoriaNavigation).FirstOrDefault(x => x.Nombre == nombre);
            if (platillo == null)
                return RedirectToAction("Index");
            return View(platillo);
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IniciarSesion(Login login)
        {
            if (login.UserName == "Dariela" && login.Password == "judith")
            {
                //Para acceder hay que hacer las siguientes 3 etapas
                //Crear claims
                //crear identidad
                //Autenticar
                //Claims
                var listaclaims = new List<Claim>()
                {
                    new Claim("Id","5"),
                    new Claim("Carrera","Sistemas"),
                    new Claim(ClaimTypes.Name,"Dariela"),
                    new Claim(ClaimTypes.Role,"Administrador") //Esto es para la impersonalizacion

                };
                //crear identidad
                var identidad = new ClaimsIdentity(listaclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                //autenticar
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidad));
                return RedirectToAction("Index", "Home", new { Area = "Administrador" }); //index, controlador, area
            }
            else
            {
                ModelState.AddModelError("", "Nombre de usuario o contraseña incorrecta");
                return View(login);
            }
        }
        public IActionResult CerrarSesion()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
