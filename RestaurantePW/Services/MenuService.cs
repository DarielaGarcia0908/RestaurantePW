using RestaurantePW.Models;

namespace RestaurantePW.Services
{
    public class MenuService
    {
        supertacoContext stcx; // Esto lo creamos para poder guardar el context y hacer uso de el en toda la clase
        //Inyection de dependencias
        public MenuService(supertacoContext context)
        {
            stcx = context;
        }
        public IEnumerable<Categoria> Get()
        {
            return stcx.Categorias.Where(x => x.Eliminado == false).OrderBy(x => x.Nombre);
        }
    }
}