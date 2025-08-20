using System.Data.SqlClient;
using System.Data;

namespace Datos
{
    public class ProductoDatos
    {
        private readonly string _connectionString = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";

        public List<Dictionary<string, object>> ObtenerProductos()
        {
            var productos = new List<Dictionary<string, object>>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("ObtenerProductos", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var producto = new Dictionary<string, object>();
                    producto["IdProducto"] = reader["IdProducto"];
                    producto["Nombre"] = reader["Nombre"];
                    producto["Descripcion"] = reader["Descripcion"];
                    producto["Precio"] = reader["Precio"];
                    producto["ImagenUrl"] = reader["ImagenUrl"];
                    producto["Categoria"] = reader["Categoria"];

                    productos.Add(producto);
                }

                reader.Close();
            }

            return productos;
        }
    }
}
