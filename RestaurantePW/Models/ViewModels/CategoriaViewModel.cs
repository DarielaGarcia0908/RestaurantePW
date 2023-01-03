namespace RestaurantePW.Models.ViewModels
{
    public class CategoriaViewModel
    {
        public string? NombreCategoria { get; set; } = "";
        public IEnumerable<Platillo>? Platillos { get; set; }
    }

}
