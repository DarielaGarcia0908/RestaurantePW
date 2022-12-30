using System;
using System.Collections.Generic;

namespace RestaurantePW.Models
{
    public partial class Platillo
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdCategoria { get; set; }
        public decimal? Precio { get; set; }
        public string? Descripcion { get; set; }

        public virtual Categoria? IdCategoriaNavigation { get; set; }
    }
}
