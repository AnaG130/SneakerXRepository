using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Datos
{
    public static class EncriptadorAES
    {
        private static readonly string clave = "1234567890123456";  // Mínimo 16 caracteres
        private static readonly string iv = "6543210987654321";      // Exactamente 16 caracteres

        public static string Encriptar(string textoPlano)
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(clave);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            using (Aes aes = Aes.Create())
            {
                aes.Key = claveBytes;
                aes.IV = ivBytes;

                ICryptoTransform encriptador = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encriptador, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(textoPlano);
                    sw.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Desencriptar(string textoEncriptado)
        {
            byte[] claveBytes = Encoding.UTF8.GetBytes(clave);
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
            byte[] buffer = Convert.FromBase64String(textoEncriptado);

            using (Aes aes = Aes.Create())
            {
                aes.Key = claveBytes;
                aes.IV = ivBytes;

                ICryptoTransform desencriptador = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(buffer))
                using (CryptoStream cs = new CryptoStream(ms, desencriptador, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
