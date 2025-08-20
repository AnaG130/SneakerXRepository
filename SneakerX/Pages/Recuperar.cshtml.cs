// Pages/Recuperar.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Negocio;
using Negocios;
using System.Text;
using System.Text.Encodings.Web;

namespace SneakerX.Pages
{
    public class RecuperarModel : PageModel
    {
        [BindProperty] public string Correo { get; set; }
        public string Mensaje { get; set; }

        private readonly EmailService emailService = new EmailService();
        private readonly UsuariosNegocio negocio = new UsuariosNegocio();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Correo))
            {
                Mensaje = "Debe ingresar su correo.";
                return Page();
            }

            bool correoExiste = negocio.CorreoExiste(Correo);

            if (!correoExiste)
            {
                Mensaje = "El correo ingresado no est? registrado.";
                return Page();
            }

            // Generar token codificado para evitar inyecciones
            string token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(Correo));
            string url = Url.PageLink("/VerificarPreguntas", null, new { token });

            // Enviar enlace por correo
            emailService.EnviarLinkRecuperacion(Correo, url);

            Mensaje = "Se ha enviado un enlace de recuperaci?n a su correo electr?nico.";
            return Page();
        }
    }
}
