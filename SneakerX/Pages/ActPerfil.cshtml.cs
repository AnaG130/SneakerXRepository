using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocios;

namespace SneakerX.Pages
{
    public class ActPerfilModel : PageModel
    {
        [BindProperty] public string Correo { get; set; }
        [BindProperty] public string Usuario { get; set; }
        [BindProperty] public string Contrasena { get; set; }
        [BindProperty] public string Confirmacion { get; set; }

        public void OnPost()
        {
            // ID fijo de ejemplo; debe venir de sesi?n o par?metro real
            int idUsuario = 305480437;

            if (Contrasena == Confirmacion)
            {
                var negocio = new UsuariosNegocio();
                negocio.ActualizarUsuario(idUsuario, Correo, Usuario, Contrasena);
            }
        }
    }
}
