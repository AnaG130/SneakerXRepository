using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SneakerX.Pages
{
    public class MetodoPagoModel : PageModel
    {
        [BindProperty] public string NumeroTarjeta { get; set; }
        [BindProperty] public decimal Saldo { get; set; }
        [BindProperty] public string FechaVencimiento { get; set; }
        [BindProperty] public string CVV { get; set; }
        [BindProperty] public string NombrePropietario { get; set; }
        [BindProperty] public string TelefonoPropietario { get; set; }

        public string Mensaje { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            string cedula = HttpContext.Session.GetString("Identificacion");

            if (string.IsNullOrEmpty(cedula))
            {
                Mensaje = "Debe iniciar sesión para registrar un método de pago.";
                return Page();
            }

            var tarjetaData = new
            {
                numeroTarjeta = NumeroTarjeta,
                saldo = Saldo,
                fechaVencimiento = FechaVencimiento,
                cvv = CVV,
                cedulaDueno = cedula,
                nombrePropietario = NombrePropietario,
                telefonoPropietario = TelefonoPropietario
            };

            using (var client = new HttpClient())
            {
                var json = JsonSerializer.Serialize(tarjetaData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:3001/tarjetas", content);

                if (response.IsSuccessStatusCode)
                {
                    // Éxito
                    return RedirectToPage("/MenuPrincipal");
                }
                else
                {
                    // Fallo
                    Mensaje = "Ocurrió un error al registrar la tarjeta.";
                    return Page();
                }
            }
        }
    }
}
