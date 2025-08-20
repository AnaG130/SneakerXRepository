using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Negocio;

namespace SneakerX.Pages
{
    public class CarritoModel : PageModel
    {
        public List<Dictionary<string, object>> Detalles { get; set; } = new();
        public decimal Total { get; set; }

        // Instancia de la capa de negocio
        private PedidoNegocio _pedidoNegocio = new PedidoNegocio();

        // Obtenemos el ID del usuario desde la sesión
        public int IdUsuario => Convert.ToInt32(HttpContext.Session.GetInt32("Identificacion"));

        public void OnGet()
        {
            // Obtener los detalles del carrito (productos del pedido)
            string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";

            using var cn = new SqlConnection(conexion);
            string query = @"
                SELECT p.Nombre, dp.Cantidad, dp.PrecioUnitario, p.ImagenUrl
                FROM DetallePedido dp
                JOIN Pedidos ped ON dp.IdPedido = ped.IdPedido
                JOIN Productos p ON dp.IdProducto = p.IdProducto
                WHERE ped.IdUsuario = @IdUsuario AND ped.Estado = 'Pendiente'";

            using var cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@IdUsuario", IdUsuario);
            cn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var detalle = new Dictionary<string, object>
                {
                    ["Nombre"] = reader["Nombre"],
                    ["Cantidad"] = reader["Cantidad"],
                    ["Precio"] = reader["PrecioUnitario"],
                    ["ImagenUrl"] = reader["ImagenUrl"]
                };
                Detalles.Add(detalle);
                Total += Convert.ToDecimal(reader["PrecioUnitario"]) * Convert.ToInt32(reader["Cantidad"]);
            }
        }

        // Método para aprobar el pedido
        public IActionResult OnPostAprobarPedido()
        {
            try
            {
                // Llamar al método de negocio para aprobar el pedido
                _pedidoNegocio.AprobarPedido(IdUsuario);

                return new JsonResult(new { success = true, message = "Pedido aprobado correctamente" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message }) { StatusCode = 500 };
            }
        }

        // Método que maneja la eliminación de un producto del carrito
        public IActionResult OnPostEliminar(string nombreProducto)
        {
            try
            {
                // Llamar al método de negocio para eliminar el detalle
                _pedidoNegocio.EliminarDetalle(IdUsuario, nombreProducto);

                return RedirectToPage(); // Refresca el carrito después de eliminar el producto
            }
            catch (Exception ex)
            {
                // Manejar el error apropiadamente
                Console.WriteLine($"Error al eliminar producto: {ex.Message}");
                return RedirectToPage();
            }
        }
    }
}