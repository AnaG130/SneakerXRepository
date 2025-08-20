using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio;
using Negocios;

namespace SneakerX.Pages
{
    public class MenuPrincipalModel : PageModel
    {
        public List<Dictionary<string, object>> Productos { get; set; }
        public int IdUsuario => Convert.ToInt32(HttpContext.Session.GetInt32("Identificacion"));
        public void OnGet()
        {
            var productoNegocio = new ProductoNegocio();
            Productos = productoNegocio.ObtenerProductos();
        }

        public IActionResult OnPostAgregarProducto(int idProducto)
        {
            int idUsuario = IdUsuario; // valor fijo para el usuario
            var pedidoNegocio = new PedidoNegocio();
            pedidoNegocio.AgregarProductoAlPedido(idUsuario, idProducto);
            return RedirectToPage("/Carrito");
        }
    }
}
