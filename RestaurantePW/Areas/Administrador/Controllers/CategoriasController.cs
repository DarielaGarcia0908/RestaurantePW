using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantePW.Models;

namespace RestaurantePW.Areas.Administrador.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Area("Administrador")]
    public class CategoriasController : Controller
    {

        private supertacoContext context;
        public CategoriasController(supertacoContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var categorias = context.Categorias.Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
            return View(categorias);
        }
        //GET
        public IActionResult Agregar()
        {
            return View();
        }
        //POST
        [HttpPost]
        public IActionResult Agregar(Categoria c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                ModelState.AddModelError("", "El nombre de la categoria no deberia de estar vacia");
            if (context.Categorias.Any(x => x.Nombre == c.Nombre))
                ModelState.AddModelError("", "Ya existe una categoria con el mismo nombre");
            if (ModelState.IsValid)
            {
                context.Add(c);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
           

            return View(c);
        }
        //GET
        public IActionResult Editar(int id)
        {
            var categoria = context.Categorias.Find(id);
            if (categoria == null)
                return RedirectToAction("Index");
            return View(categoria);
        }
        //POST
        [HttpPost]
        public IActionResult Editar(Categoria c)
        {
            if (string.IsNullOrWhiteSpace(c.Nombre))
                ModelState.AddModelError("", "Favor de escribir el nombre de la categoria");
            if (context.Categorias.Any(x => x.Nombre == c.Nombre && x.Id != c.Id))
                ModelState.AddModelError("", "Favor de escribir el nombre de la categoria");
            if (ModelState.IsValid)
            {
                var categoria = context.Categorias.Find(c.Id);
                if (categoria == null)
                    return RedirectToAction("Index");
                categoria.Nombre = c.Nombre;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(c);
        }
        //GET
        public IActionResult Eliminar(int id)
        {
            var categoria = context.Categorias.Find(id);
            if (categoria == null)
                return RedirectToAction("Index");
            return View(categoria);
        }
        //POST
        [HttpPost]
        public IActionResult Eliminar(Categoria c)
        {
            var categoria = context.Categorias.Find(c.Id);
            if (categoria == null)
                ModelState.AddModelError("", "La categoria no existe o ya ha sido eliminada");
            else
            {
                if (context.Platillos.Any(x => x.IdCategoria == c.Id))
                    ModelState.AddModelError("", "La categoria no se puede eliminar debido a que tiene productos");
                if (ModelState.IsValid)
                {
                    context.Remove(categoria);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(categoria);
        }
    }
}
