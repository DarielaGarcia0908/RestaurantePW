using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantePW.Areas.Administrador.Models;
using RestaurantePW.Models;
using System.Data;

namespace RestaurantePW.Areas.Administrador.Controllers
{
    [Authorize(Roles = "Administrador, Supervisor")]
    [Area("Administrador")]
    public class PlatillosController : Controller
    {
        private readonly supertacoContext context;
        private readonly IWebHostEnvironment em;
        public PlatillosController(supertacoContext context, IWebHostEnvironment em)
        {
            this.context = context;
            this.em = em;
        }
        public IActionResult Index(IndexPlatillosVM vm)
        {
            if (vm.IdCategoria == 0)//Esto sera en caso de que no hayan seleccionado nada en el combobox
                vm.Platillos = context.Platillos.Include(x => x.IdCategoriaNavigation).OrderBy(x => x.Nombre);
            else
                vm.Platillos = context.Platillos.Include(x => x.IdCategoriaNavigation).Where(x => x.IdCategoria == vm.IdCategoria).OrderBy(x => x.Nombre);
            vm.Categorias = context.Categorias.OrderBy(x => x.Nombre);
            return View(vm);
        }
    }
}
