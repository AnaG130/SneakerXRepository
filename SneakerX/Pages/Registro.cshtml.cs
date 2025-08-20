// Pages/Registro.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Negocios;
using Negocio;
using Datos;

namespace SneakerX.Pages
{
    public class RegistroModel : PageModel
    {
        [BindProperty] public int Id { get; set; }
        [BindProperty] public string Nombre { get; set; }
        [BindProperty] public string Apellido1 { get; set; }
        [BindProperty] public string Apellido2 { get; set; }
        [BindProperty] public string Correo { get; set; }
        [BindProperty] public string Usuario { get; set; }
        [BindProperty] public string Password { get; set; }
        [BindProperty] public string Confirmar { get; set; }
        [BindProperty] public string Pregunta1 { get; set; }
        [BindProperty] public string Respuesta1 { get; set; }
        [BindProperty] public string Pregunta2 { get; set; }
        [BindProperty] public string Respuesta2 { get; set; }
        [BindProperty] public string Pregunta3 { get; set; }
        [BindProperty] public string Respuesta3 { get; set; }

        private readonly UbicacionesNegocio _ubicacionesNegocio;
        public RegistroModel(UbicacionesNegocio ubicacionesNegocio)
        {
            _ubicacionesNegocio = ubicacionesNegocio;
        }
        public List<Ubicacion> Paises { get; set; }
        public string Mensaje { get; set; }

        private readonly EmailService emailService = new EmailService();

        public void OnGet()
        {
            // Cargar los pa?ses al inicio
            Paises = _ubicacionesNegocio.ObtenerPaises();
        }
        // Esta acci?n ser? llamada por AJAX para cargar provincias por pa?s
        public IActionResult OnGetProvincias(int paisId)
        {
            var provincias = _ubicacionesNegocio.ObtenerProvincias(paisId);
            return new JsonResult(provincias);
        }

        // Esta acci?n ser? llamada por AJAX para cargar cantones por provincia
        public IActionResult OnGetCantones(int provinciaId)
        {
            var cantones = _ubicacionesNegocio.ObtenerCantones(provinciaId);
            return new JsonResult(cantones);
        }

        // Esta acci?n ser? llamada por AJAX para cargar distritos por cant?n
        public IActionResult OnGetDistritos(int cantonId)
        {
            var distritos = _ubicacionesNegocio.ObtenerDistritos(cantonId);
            return new JsonResult(distritos);
        }

        public IActionResult OnPost()
        {
            var negocio = new UsuariosNegocio();

            // Obtener la direcci?n concatenada
            string direccion = $"{Request.Form["pais"]}, {Request.Form["provincia"]}, {Request.Form["canton"]}, {Request.Form["distrito"]}";

            var resultado = negocio.RegistrarUsuario(Id, Nombre, Apellido1, Apellido2, Correo, Usuario, Password, Confirmar,
                Pregunta1, Respuesta1, Pregunta2, Respuesta2, Pregunta3, Respuesta3, direccion);

            if (resultado == "OK")
            {
                // Generar token de verificaci?n
                string token = new Random().Next(100000, 999999).ToString();
                HttpContext.Session.SetString("TokenAuth", token);
                HttpContext.Session.SetString("CorreoUsuario", Correo);

                // Enviar el token al correo del usuario
                emailService.EnviarToken(Correo, token);

                return RedirectToPage("/Autenticacion");
            }

            Mensaje = resultado;
            return Page();
        }
    }
}