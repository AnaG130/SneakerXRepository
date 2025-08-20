// Archivo: Negocio/EmailService.cs
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;


namespace Negocio
{
    public class EmailService
    {
        public void EnviarToken(string destinatario, string token)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sneaker X", "l.qchavesq.30@gmail.com"));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = "Código de Autenticación - Sneaker X";

            mensaje.Body = new TextPart("plain")
            {
                Text = $"Tu token de autenticación es: {token}"
            };

            using var cliente = new SmtpClient();
            cliente.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            cliente.Authenticate("l.qchavesq.30@gmail.com", "tztd grwl fxid vqcw");
            cliente.Send(mensaje);
            cliente.Disconnect(true);
        }
        public void EnviarLinkRecuperacion(string destinatario, string enlace)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sneaker X", "l.qchavesq.30@gmail.com"));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = "Recuperación de Contraseña - Sneaker X";

            mensaje.Body = new TextPart("plain")
            {
                Text = $"Hola,\n\nHemos recibido una solicitud para restablecer tu contraseña.\n\n" +
                       $"Haz clic en el siguiente enlace para continuar con la verificación de identidad:\n\n{enlace}\n\n" +
                       "Si no solicitaste este cambio, por favor ignora este mensaje.\n\nSaludos,\nEquipo de Sneakers X"
            };

            using var cliente = new MailKit.Net.Smtp.SmtpClient();
            cliente.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            cliente.Authenticate("l.qchavesq.30@gmail.com", "tztd grwl fxid vqcw"); // Tu contraseña de aplicación
            cliente.Send(mensaje);
            cliente.Disconnect(true);
        }
    }
}