using System;
using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class PagosDatos
    {
        private string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";

        // Realiza una transacción mediante SINPE (pago entre teléfonos)
        public void RealizarSINPE(string telefonoEmisor, string telefonoReceptor, decimal monto, string detalle)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("Luis.RealizarSINPE", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@TelefonoEmisor", telefonoEmisor);
            cmd.Parameters.AddWithValue("@TelefonoReceptor", telefonoReceptor);
            cmd.Parameters.AddWithValue("@Monto", monto);
            cmd.Parameters.AddWithValue("@Detalle", detalle);

            cn.Open();
            cmd.ExecuteNonQuery();
        }

        // Obtiene las tarjetas asociadas a una cédula de usuario
        public DataTable ObtenerTarjetasPorCedula(string cedulaDueno)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("Luis.ObtenerTarjetasPorCedula", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CedulaDueno", cedulaDueno);

            var dataTable = new DataTable();
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(dataTable);
            }

            return dataTable;
        }

        // Realiza una compra con tarjeta (actualiza el saldo y registra la transacción)
        public void RealizarCompraTarjeta(string numeroTarjeta, decimal monto, string detalle)
        {
            using var cn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("Luis.RealizarCompraTarjeta", cn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@NumeroTarjeta", numeroTarjeta);
            cmd.Parameters.AddWithValue("@Monto", monto);
            cmd.Parameters.AddWithValue("@Detalle", detalle);

            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
