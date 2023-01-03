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
    }
}
