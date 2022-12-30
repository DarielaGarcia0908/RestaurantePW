using System;
using System.Collections.Generic;

namespace RestaurantePW.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Platillos = new HashSet<Platillo>();
        }

        public int Id { get; set; }
        public string? Nombre { get; set; }
        public bool Eliminado { get; set; }

        public virtual ICollection<Platillo> Platillos { get; set; }
    }
}
