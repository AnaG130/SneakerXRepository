using Datos;

namespace Negocios
{
    public class UsuariosNegocio
    {
        private readonly UsuariosDatos datos = new UsuariosDatos();

        private bool CumplePoliticas(string contrasena, out string mensaje)
        {
            mensaje = "";

            if (string.IsNullOrEmpty(contrasena))
            {
                mensaje = "La contraseña no puede estar vacía.";
                return false;
            }

            bool tieneMayuscula = contrasena.Any(char.IsUpper);
            bool tieneMinuscula = contrasena.Any(char.IsLower);
            bool tieneNumero = contrasena.Any(char.IsDigit);
            bool tieneEspecial = contrasena.Any(c => !char.IsLetterOrDigit(c));
            bool tieneLongitud = contrasena.Length >= 8;

            if (!tieneMayuscula) mensaje += "Debe tener al menos una mayúscula. ";
            if (!tieneMinuscula) mensaje += "Debe tener al menos una minúscula. ";
            if (!tieneNumero) mensaje += "Debe tener al menos un número. ";
            if (!tieneEspecial) mensaje += "Debe tener al menos un carácter especial. ";
            if (!tieneLongitud) mensaje += "Debe tener al menos 8 caracteres. ";

            return tieneMayuscula && tieneMinuscula && tieneNumero && tieneEspecial && tieneLongitud;
        }


        public string RegistrarUsuario(int identificacion, string nombre, string apellido1, string apellido2, string correo, string usuario, string contrasena, string confirmar,
                                       string pregunta1, string respuesta1, string pregunta2, string respuesta2, string pregunta3, string respuesta3, string direccion)
        {
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasena))
                return "Los campos obligatorios no pueden estar vacíos.";

            if (contrasena != confirmar)
                return "Las contraseñas no coinciden.";

            if (!CumplePoliticas(contrasena, out string mensajePoliticas))
                return $"La contraseña no cumple las políticas: {mensajePoliticas}";

            try
            {
                datos.InsertarUsuario(identificacion, nombre, apellido1, apellido2, correo, usuario, contrasena, direccion);
                datos.InsertarPreguntasUsuario(identificacion, pregunta1, respuesta1, pregunta2, respuesta2, pregunta3, respuesta3);
                return "OK";
            }
            catch (Exception ex)
            {
                return $"Error al registrar usuario: {ex.Message}";
            }
        }


        public void ActualizarUsuario(int id, string correo, string usuario, string contrasena)
        {
            datos.ActualizarUsuario(id, correo, usuario, contrasena);
        }

        public string IniciarSesion(string usuario, string contrasena)
        {
            string nombreCompleto;
            int identificacion;
            bool esValido = datos.ValidarLogin(usuario, contrasena, out nombreCompleto, out identificacion);

            if (esValido)
            {
                // Al validar el login, ahora también pasamos la identificación
                return $"Bienvenido, {nombreCompleto}|{identificacion}";
            }
            else
            {
                return "Usuario o contraseña incorrectos.";
            }
        }

        public string ObtenerCorreoUsuario(string usuario)
        {
            return datos.ObtenerCorreoPorUsuario(usuario);
        }
        public bool CorreoExiste(string correo)
        {
            return datos.CorreoExiste(correo);
        }
        public string[] ObtenerPreguntas(string correo)
        {
            return datos.ObtenerPreguntas(correo);
        }

        public bool VerificarRespuestas(string correo, string r1, string r2, string r3)
        {
            return datos.VerificarRespuestas(correo, r1, r2, r3);
        }

        public void ActualizarPassword(string correo, string nuevaPassword)
        {
            datos.ActualizarPassword(correo, nuevaPassword);
        }


    }
}
