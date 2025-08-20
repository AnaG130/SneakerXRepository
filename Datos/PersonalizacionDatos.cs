using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class PersonalizacionDatos
    {
        private string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";
        public void InsertarPersonalizacion(int idUsuario, int idProducto, string colorPrincipal, string colorSecundario, string colorCordones, decimal precioFinal)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.InsertarPersonalizacion", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cmd.Parameters.AddWithValue("@ColorPrincipal", colorPrincipal);
            cmd.Parameters.AddWithValue("@ColorSecundario", colorSecundario);
            cmd.Parameters.AddWithValue("@ColorCordones", colorCordones);
            cmd.Parameters.AddWithValue("@PrecioFinal", precioFinal);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
