// Pages/Autenticacion.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SneakerX.Pages
{
    public class AutenticacionModel : PageModel
    {
        [BindProperty] public string TokenIngresado { get; set; }
        public string Mensaje { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string tokenCorrecto = HttpContext.Session.GetString("TokenAuth");

            if (tokenCorrecto == null)
            {
                Mensaje = "Sesión expirada o token no generado. Por favor, inicie sesión de nuevo.";
                return Page();
            }

            if (TokenIngresado == tokenCorrecto)
            {
                // Token correcto, redirigir a menú principal
                return RedirectToPage("/MenuPrincipal");
            }
            else
            {
                Mensaje = "Token inválido. Intente nuevamente.";
                return Page();
            }
        }
    }
}
