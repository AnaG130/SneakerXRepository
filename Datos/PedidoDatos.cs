using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace Datos
{
    public class PedidoDatos
    {
        private string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";

        public int ObtenerPedidoPendiente(int idUsuario)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("ObtenerOPedidoPendiente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            cn.Open();

            var result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public int CrearPedidoPendiente(int idUsuario)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("Luis.CrearPedidoPendiente", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);

            var outputParam = new SqlParameter("@IdPedido", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputParam);

            cn.Open();
            cmd.ExecuteNonQuery();

            return (int)outputParam.Value;
        }

        public void AgregarDetallePedido(int idPedido, int idProducto)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.AgregarDetallePedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdPedido", idPedido);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void AprobarPedido(int idUsuario)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.AprobarPedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void EliminarDetallePedido(int idUsuario, string nombreProducto)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.EliminarDetallePedido", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            cmd.Parameters.AddWithValue("@NombreProducto", nombreProducto);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

    }
}
