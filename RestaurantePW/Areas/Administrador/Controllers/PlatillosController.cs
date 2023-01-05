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
        public IActionResult Agregar()
        {
            PlatillosViewModel vm = new PlatillosViewModel();

            vm.Categorias = context.Categorias.OrderBy(x => x.Nombre);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(PlatillosViewModel vm)
        {
            //Tendremos que validar en el proyecto final todo lo mejor posible, aqui sera una sencilla pero no olvidar esto
            if (vm.Platillo != null)
            {
                if (string.IsNullOrWhiteSpace(vm.Platillo.Nombre))
                    ModelState.AddModelError("", "Escribe el nombre del Platillo");
                if (context.Platillos.Any(x => x.Nombre == vm.Platillo.Nombre))
                    ModelState.AddModelError("", "Ya existe un platillo con el mismo nombre");

                if (string.IsNullOrWhiteSpace(vm.Platillo.Descripcion))
                    ModelState.AddModelError("", "Escribe la descripcion del platillo");
                if (vm.Platillo.IdCategoria==0)
                    ModelState.AddModelError("", "Seleccione una categoría");
                if (vm.Platillo.Precio <=0)
                    ModelState.AddModelError("", "Escribe el precio del Platillo");

                if (ModelState.IsValid)
                {
                    //Agregar y guardar cambios a la bd
                    context.Add(vm.Platillo);
                    context.SaveChanges();
                    //agregar la imagen al Platillo (le ponemos la imagen no disponible)
                    if (vm.Imagen == null)
                    {
                        string nodisp = em.WebRootPath + "/img_platillos/0.png";
                        string nuevaimg = em.WebRootPath + $"/img_platillos/{vm.Platillo.Id}.jpg";
                        System.IO.File.Copy(nodisp, nuevaimg, true);
                    }
                    else
                    {
                        string nuevaimg = em.WebRootPath + $"/img_platillos/{vm.Platillo.Id}.jpg";
                        var archivo = System.IO.File.Create(nuevaimg);
                        vm.Imagen.CopyTo(archivo);
                        archivo.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            vm.Categorias = context.Categorias.OrderBy(x => x.Nombre);
            return View(vm);
        }

        public IActionResult Editar(int id)
        {
            var p = context.Platillos.Find(id);
            if (p == null)
                return RedirectToAction("Index");
            PlatillosViewModel vm = new PlatillosViewModel();
            vm.Platillo = p;
            vm.Categorias = context.Categorias.OrderBy(x => x.Nombre);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(PlatillosViewModel vm)
        {
            if (vm.Platillo != null)
            {
                var p = context.Platillos.Find(vm.Platillo.Id);
                if (p == null)
                    return RedirectToAction("Index");
                //Validacion, hacer las demas despues
                if (string.IsNullOrWhiteSpace(vm.Platillo.Nombre))
                    ModelState.AddModelError("", "Escribe el nombre del Platillo");
                
                if (string.IsNullOrWhiteSpace(vm.Platillo.Descripcion))
                    ModelState.AddModelError("", "Escribe la descripcion del platillo");
               
                if (vm.Platillo.Precio <= 0)
                    ModelState.AddModelError("", "Escribe el precio del Platillo");
                if (ModelState.IsValid)
                {
                    p.Nombre = vm.Platillo.Nombre;
                    p.Descripcion = vm.Platillo.Descripcion;
                    p.IdCategoria = vm.Platillo.IdCategoria;
                    p.Precio = vm.Platillo.Precio;
                    context.SaveChanges();
                }
                if (vm.Imagen != null)
                {
                    string ruta = em.WebRootPath + $"/img_platillos/{vm.Platillo.Id}.jpg";
                    var archivo = System.IO.File.Create(ruta);
                    vm.Imagen.CopyTo(archivo);
                    archivo.Close();
                }
                return RedirectToAction("Index");
            }
            vm.Categorias = context.Categorias.OrderBy(x => x.Nombre);
            return View(vm);
        }

        public IActionResult Eliminar(int id)
        {
            var Platillo = context.Platillos.Find(id);
            if (Platillo == null)
                return RedirectToAction("Index");
            return View(Platillo);
        }
        [HttpPost]
        public IActionResult Eliminar(Platillo p)
        {
            var Platillo = context.Platillos.Find(p.Id);
            if (Platillo == null)
                ModelState.AddModelError("", "El Platillo no existe o ya ha sido eliminado.");
            else
            {
                string ruta = em.WebRootPath + $"/img_platillos/{Platillo.Id}.jpg";
                context.Remove(Platillo);
                context.SaveChanges();
                System.IO.File.Delete(ruta);
                return RedirectToAction("Index");
            }
            return View(p);
        }
    }
}
