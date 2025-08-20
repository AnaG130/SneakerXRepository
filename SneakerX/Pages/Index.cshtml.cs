using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Negocio;
using Negocios;

namespace SneakerX.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UsuariosNegocio negocio = new UsuariosNegocio();
        private readonly EmailService emailService = new EmailService();

        [BindProperty] public string Usuario { get; set; }
        [BindProperty] public string Contrasena { get; set; }
        public string Mensaje { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            string intentosKey = $"Intentos_{Usuario}";
            string bloqueoKey = $"Bloqueo_{Usuario}";

            int intentosFallidos = HttpContext.Session.GetInt32(intentosKey) ?? 0;
            DateTime? bloqueoHasta = HttpContext.Session.GetString(bloqueoKey) != null
                ? DateTime.Parse(HttpContext.Session.GetString(bloqueoKey))
                : (DateTime?)null;

            if (bloqueoHasta.HasValue && DateTime.Now < bloqueoHasta.Value)
            {
                Mensaje = $"Cuenta bloqueada. Inténtelo después de {bloqueoHasta.Value:HH:mm:ss}";
                return Page();
            }

            Mensaje = negocio.IniciarSesion(Usuario, Contrasena);

            // Verificamos si el login fue exitoso
            if (Mensaje.StartsWith("Bienvenido"))
            {
                // Obtener los detalles del mensaje para separar el nombre completo y la identificación
                var partes = Mensaje.Split('|');
                string nombreCompleto = partes[0];
                int identificacion = int.Parse(partes[1]);

                // Guardar la identificación en la sesión
                HttpContext.Session.SetInt32("Identificacion", identificacion);

                HttpContext.Session.Remove(intentosKey);
                HttpContext.Session.Remove(bloqueoKey);

                // Generar y guardar token
                string token = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("TokenAuth", token);

                // Obtener el correo desde negocio
                string correo = negocio.ObtenerCorreoUsuario(Usuario);

                // Enviar token por correo
                emailService.EnviarToken(correo, token);

                return RedirectToPage("/Autenticacion");
            }
            else
            {
                intentosFallidos++;
                HttpContext.Session.SetInt32(intentosKey, intentosFallidos);

                if (intentosFallidos >= 3)
                {
                    DateTime finBloqueo = DateTime.Now.AddMinutes(2);
                    HttpContext.Session.SetString(bloqueoKey, finBloqueo.ToString());
                    Mensaje = $"Has fallado 3 veces. La cuenta queda bloqueada por 2 minutos (hasta {finBloqueo:HH:mm:ss})";
                }
                else
                {
                    Mensaje += $" - Intento {intentosFallidos}/3";
                }

                return Page();
            }
        }
    }
}
