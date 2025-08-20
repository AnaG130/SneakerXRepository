using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Collections.Generic;
using Negocios;
using Negocio;

namespace SneakerX.Pages
{
    public class PersonalizacionModel : PageModel
    {
        public List<Dictionary<string, object>> Productos { get; set; } = new();
        public List<string> Colores { get; } = new() { "Color original", "Rojo", "Azul", "Verde", "Negro", "Blanco" };
        [BindProperty] public int IdProducto { get; set; }
        [BindProperty] public string ColorPrincipal { get; set; }
        [BindProperty] public string ColorSecundario { get; set; }
        [BindProperty] public string ColorCordones { get; set; }
        public decimal PrecioBase { get; set; }
        public decimal PrecioFinal { get; set; }

        private readonly int idUsuario = 305480437;

        public void OnGet()
        {
            string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";
            using var cn = new SqlConnection(conexion);
            string query = "SELECT IdProducto, Nombre, Precio FROM Productos";

            using var cmd = new SqlCommand(query, cn);
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Productos.Add(new Dictionary<string, object>
                {
                    ["IdProducto"] = reader["IdProducto"],
                    ["Nombre"] = reader["Nombre"],
                    ["Precio"] = reader["Precio"]
                });
            }
        }

        public IActionResult OnPost()
        {
            // Costo adicional por cambio de color
            decimal adicional = 0;
            if (ColorPrincipal != "Color original") adicional += 2000;
            if (ColorSecundario != "Color original") adicional += 1500;
            if (ColorCordones != "Color original") adicional += 1000;

            // Obtener precio base
            string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("SELECT Precio FROM Productos WHERE IdProducto = @id", cn);
            cmd.Parameters.AddWithValue("@id", IdProducto);
            cn.Open();
            PrecioBase = Convert.ToDecimal(cmd.ExecuteScalar());

            PrecioFinal = PrecioBase + adicional;

            var negocio = new PersonalizacionNegocio();
            negocio.GuardarPersonalizacion(idUsuario, IdProducto, ColorPrincipal, ColorSecundario, ColorCordones, PrecioFinal);

            return RedirectToPage("/Carrito");
        }
    }
}
