using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace RestaurantePW.Areas.Administrador.Controllers
{
    [Authorize]
    [Area("Administrador")]

    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
