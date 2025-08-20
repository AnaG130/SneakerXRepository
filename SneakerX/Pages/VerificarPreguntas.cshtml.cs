// Pages/VerificarPreguntas.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using Negocios;

namespace SneakerX.Pages
{
    public class VerificarPreguntasModel : PageModel
    {
        [BindProperty] public string Respuesta1 { get; set; }
        [BindProperty] public string Respuesta2 { get; set; }
        [BindProperty] public string Respuesta3 { get; set; }
        [BindProperty] public string NuevaPassword { get; set; }
        [BindProperty] public string ConfirmarPassword { get; set; }

        public string Mensaje { get; set; }
        public string Pregunta1 { get; set; }
        public string Pregunta2 { get; set; }
        public string Pregunta3 { get; set; }

        private string Correo;
        private readonly UsuariosNegocio negocio = new UsuariosNegocio();

        public IActionResult OnGet(string token)
        {
            if (string.IsNullOrEmpty(token)) return RedirectToPage("/Index");

            Correo = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            HttpContext.Session.SetString("CorreoRecuperacion", Correo);

            var preguntas = negocio.ObtenerPreguntas(Correo);
            if (preguntas == null || preguntas.Length != 3) return RedirectToPage("/Index");

            Pregunta1 = preguntas[0];
            Pregunta2 = preguntas[1];
            Pregunta3 = preguntas[2];

            return Page();
        }

        public IActionResult OnPost()
        {
            Correo = HttpContext.Session.GetString("CorreoRecuperacion");
            if (string.IsNullOrEmpty(Correo)) return RedirectToPage("/Index");

            bool respuestasCorrectas = negocio.VerificarRespuestas(Correo, Respuesta1, Respuesta2, Respuesta3);
            if (!respuestasCorrectas)
            {
                Mensaje = "Las respuestas no coinciden. Intente nuevamente.";
                return Page();
            }

            if (NuevaPassword != ConfirmarPassword)
            {
                Mensaje = "Las contrase�as no coinciden.";
                return Page();
            }

            negocio.ActualizarPassword(Correo, NuevaPassword);
            HttpContext.Session.Remove("CorreoRecuperacion");
            return RedirectToPage("/Index");
        }
    }
}