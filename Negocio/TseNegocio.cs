using Datos;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Negocios
{
    public class TseNegocio
    {
        private readonly TseDatos datos = new TseDatos();

        /// <summary>
        /// Consulta una persona por su cédula en el TSE.
        /// </summary>
        public async Task<string> ConsultarPersonaPorCedulaAsync(string cedula)
        {
            try
            {
                string json = await datos.ConsultarPersonaPorCedula(cedula);
                using var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;
                if (root.GetProperty("success").GetBoolean())
                {
                    var persona = root.GetProperty("persona");
                    string nombreCompleto = $"{persona.GetProperty("Nombre").GetString()} {persona.GetProperty("Apellido1").GetString()} {persona.GetProperty("Apellido2").GetString()}";
                    string telefono = persona.GetProperty("Telefono").GetString();

                    return $"✅ Persona encontrada: {nombreCompleto}, Teléfono: {telefono}";
                }

                return "❌ Persona no encontrada.";
            }
            catch (Exception ex)
            {
                return $"🚫 Error al consultar persona: {ex.Message}";
            }
        }

        /// <summary>
        /// Registra una persona en el TSE.
        /// </summary>
        public async Task<string> RegistrarPersonaTSEAsync(string cedula, string nombre, string apellido1, string apellido2, string telefono)
        {
            if (string.IsNullOrWhiteSpace(cedula) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(telefono))
                return "Los campos cédula, nombre y teléfono son obligatorios.";

            try
            {
                var persona = new PersonaTseDto
                {
                    Cedula = cedula,
                    Nombre = nombre,
                    Apellido1 = apellido1,
                    Apellido2 = apellido2,
                    Telefono = telefono
                };

                await datos.InsertarPersonaTSE(persona);
                return "✅ Persona registrada correctamente en el TSE.";
            }
            catch (Exception ex)
            {
                return $"🚫 Error al registrar persona en el TSE: {ex.Message}";
            }
        }
    }
}
