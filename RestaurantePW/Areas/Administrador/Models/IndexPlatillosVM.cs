using RestaurantePW.Models;

namespace RestaurantePW.Areas.Administrador.Models
{
    public class IndexPlatillosVM
    {

        public IEnumerable<Categoria>? Categorias { get; set; }
        public IEnumerable<Platillo>? Platillos { get; set; }
        public int IdCategoria { get; set; }
    }
}
