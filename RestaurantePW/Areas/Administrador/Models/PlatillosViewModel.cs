using RestaurantePW.Models;

namespace RestaurantePW.Areas.Administrador.Models
{
    public class PlatillosViewModel
    {
        public Platillo? Platillo { get; set; }
        public IEnumerable<Categoria>? Categorias { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
