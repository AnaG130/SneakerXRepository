using System.Data;
using System.Data.SqlClient;

namespace Datos
{
    public class UsuariosDatos
    {
        private readonly string conexion = "Server=tiusr11pl.cuc-carrera-ti.ac.cr;Database=SneakersX;User ID=Luis;Password=#Kata161918;";

        public void InsertarUsuario(int identificacion, string nombre, string apellido1, string apellido2, string correo, string usuario, string contrasena, string direccion)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("anix.AgregarUsuario", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Encriptar antes de guardar
                string correoEncriptado = EncriptadorAES.Encriptar(correo);
                string usuarioEncriptado = EncriptadorAES.Encriptar(usuario);
                string contrasenaEncriptada = EncriptadorAES.Encriptar(contrasena);

                cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@PrimerApellido", apellido1);
                cmd.Parameters.AddWithValue("@SegundoApellido", apellido2);
                cmd.Parameters.AddWithValue("@CorreoElectronico", correoEncriptado);
                cmd.Parameters.AddWithValue("@Usuario", usuarioEncriptado);
                cmd.Parameters.AddWithValue("@Contrasena", contrasenaEncriptada);
                cmd.Parameters.AddWithValue("@Direccion", direccion); // Guardamos la dirección

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActualizarUsuario(int id, string correo, string usuario, string contrasena)
        {
            using (SqlConnection connection = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("anix.ActualizarUsuario", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                string correoEncriptado = EncriptadorAES.Encriptar(correo);
                string usuarioEncriptado = EncriptadorAES.Encriptar(usuario);
                string contrasenaEncriptada = EncriptadorAES.Encriptar(contrasena);

                cmd.Parameters.AddWithValue("@Identificacion", id);
                cmd.Parameters.AddWithValue("@CorreoElectronico", correoEncriptado);
                cmd.Parameters.AddWithValue("@Usuario", usuarioEncriptado);
                cmd.Parameters.AddWithValue("@Contrasena", contrasenaEncriptada);

                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool ValidarLogin(string usuario, string contrasena, out string nombreCompleto, out int identificacion)
        {
            nombreCompleto = string.Empty;
            identificacion = 0; // Inicializamos la identificación como 0

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("anix.ValidarUsuario", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Encriptar usuario y contraseña para comparar
                string usuarioEncriptado = EncriptadorAES.Encriptar(usuario);
                string contrasenaEncriptada = EncriptadorAES.Encriptar(contrasena);

                cmd.Parameters.AddWithValue("@Usuario", usuarioEncriptado);
                cmd.Parameters.AddWithValue("@Contrasena", contrasenaEncriptada);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Recuperamos la identificación
                    identificacion = Convert.ToInt32(reader["Identificacion"]);

                    string nombre = reader["Nombre"].ToString();
                    string apellido1 = reader["PrimerApellido"].ToString();
                    string apellido2 = reader["SegundoApellido"].ToString();

                    nombreCompleto = $"{nombre} {apellido1} {apellido2}";
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public void InsertarPreguntasUsuario(int identificacion, string pregunta1, string respuesta1, string pregunta2, string respuesta2, string pregunta3, string respuesta3)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("anix.AgregarPreguntasUsuario", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Encriptar preguntas y respuestas antes de guardar
                string pregunta1Enc = EncriptadorAES.Encriptar(pregunta1);
                string respuesta1Enc = EncriptadorAES.Encriptar(respuesta1);

                string pregunta2Enc = EncriptadorAES.Encriptar(pregunta2);
                string respuesta2Enc = EncriptadorAES.Encriptar(respuesta2);

                string pregunta3Enc = EncriptadorAES.Encriptar(pregunta3);
                string respuesta3Enc = EncriptadorAES.Encriptar(respuesta3);

                cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                cmd.Parameters.AddWithValue("@Pregunta1", pregunta1Enc);
                cmd.Parameters.AddWithValue("@Respuesta1", respuesta1Enc);
                cmd.Parameters.AddWithValue("@Pregunta2", pregunta2Enc);
                cmd.Parameters.AddWithValue("@Respuesta2", respuesta2Enc);
                cmd.Parameters.AddWithValue("@Pregunta3", pregunta3Enc);
                cmd.Parameters.AddWithValue("@Respuesta3", respuesta3Enc);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public string ObtenerCorreoPorUsuario(string usuario)
        {
            string correo = null;

            using (SqlConnection conn = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("anix.ObtenerCorreoPorUsuario", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                string usuarioEnc = EncriptadorAES.Encriptar(usuario);
                cmd.Parameters.AddWithValue("@Usuario", usuarioEnc);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    correo = EncriptadorAES.Desencriptar(reader["CorreoElectronico"].ToString());
                }

                conn.Close();
            }

            return correo;
        }
        public bool CorreoExiste(string correo)
        {
            using (SqlConnection conn = new SqlConnection(conexion))
            {
                string correoEnc = EncriptadorAES.Encriptar(correo);
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Luis.Usuarios WHERE CorreoElectronico = @Correo", conn);
                cmd.Parameters.AddWithValue("@Correo", correoEnc);

                conn.Open();
                int conteo = (int)cmd.ExecuteScalar();
                return conteo > 0;
            }
        }
        public string[] ObtenerPreguntas(string correo)
        {
            var preguntas = new string[3];
            using var conn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.sp_ObtenerPreguntas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Correo", correo);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                preguntas[0] = EncriptadorAES.Desencriptar(reader.GetString(0));
                preguntas[1] = EncriptadorAES.Desencriptar(reader.GetString(1));
                preguntas[2] = EncriptadorAES.Desencriptar(reader.GetString(2));
            }
            return preguntas;
        }

        public bool VerificarRespuestas(string correo, string r1, string r2, string r3)
        {
            using var conn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.sp_VerificarRespuestas", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            string correoEnc = EncriptadorAES.Encriptar(correo);
            string r1Enc = EncriptadorAES.Encriptar(r1);
            string r2Enc = EncriptadorAES.Encriptar(r2);
            string r3Enc = EncriptadorAES.Encriptar(r3);
            cmd.Parameters.AddWithValue("@Correo", correoEnc);
            cmd.Parameters.AddWithValue("@Respuesta1", r1Enc);
            cmd.Parameters.AddWithValue("@Respuesta2", r2Enc);
            cmd.Parameters.AddWithValue("@Respuesta3", r3Enc);

            conn.Open();
            var result = cmd.ExecuteScalar();
            return result != null && Convert.ToInt32(result) == 1;
        }

        public void ActualizarPassword(string correo, string nuevaPassword)
        {
            using var conn = new SqlConnection(conexion);
            using var cmd = new SqlCommand("anix.sp_ActualizarPassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            string correoEnc = EncriptadorAES.Encriptar(correo);
            string nuevaPasswordEnc = EncriptadorAES.Encriptar(nuevaPassword);
            cmd.Parameters.AddWithValue("@Correo", correoEnc);
            cmd.Parameters.AddWithValue("@NuevaPassword", nuevaPasswordEnc);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
