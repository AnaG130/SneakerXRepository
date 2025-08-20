using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Datos
{
    public class TseDatos
    {
        private readonly HttpClient _http;

        public TseDatos()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("http://localhost:3001/");
        }

        /// <summary>
        /// Consulta una persona por su cédula en el TSE.
        /// </summary>
        public async Task<string> ConsultarPersonaPorCedula(string cedula)
        {
            var response = await _http.GetAsync($"tse/{cedula}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Registra una persona nueva en el TSE.
        /// </summary>
        public async Task InsertarPersonaTSE(PersonaTseDto persona)
        {
            var contenido = new StringContent(JsonSerializer.Serialize(persona), Encoding.UTF8, "application/json");
            var response = await _http.PostAsync("tse", contenido);
            response.EnsureSuccessStatusCode();
        }
    }

    public class PersonaTseDto
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Telefono { get; set; }
    }
}
