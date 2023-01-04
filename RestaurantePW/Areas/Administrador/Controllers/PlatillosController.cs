using Microsoft.AspNetCore.Mvc;

namespace RestaurantePW.Areas.Administrador.Controllers
{
    public class PlatillosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
