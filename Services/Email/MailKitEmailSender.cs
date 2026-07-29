using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Plantilla_Base.Services.Email
{
    // Implementacion SMTP basada en MailKit.
    // AccountController la usa para enviar enlaces de recuperacion de contrasena.
    public class MailKitEmailSender : IEmailSender
    {
        private readonly SmtpSettings _settings;
        private readonly ILogger<MailKitEmailSender> _logger;

        // Lee SmtpSettings desde configuracion y habilita logs de envio/error.
        public MailKitEmailSender(IOptions<SmtpSettings> settings, ILogger<MailKitEmailSender> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        // Construye y envia el correo de recuperacion.
        // Si SMTP no esta configurado, no rompe el flujo: deja el enlace en logs
        // para pruebas locales.
        public async Task SendPasswordResetAsync(string toEmail, string resetUrl)
        {
            var builder = new BodyBuilder
            {
                HtmlBody = $"""
                    <p>Recibimos una solicitud para restablecer tu contrasena.</p>
                    <p><a href="{resetUrl}">Restablecer contrasena</a></p>
                    <p>Este enlace vence en 30 minutos. Si no solicitaste este cambio, ignora este correo.</p>
                    """,
                TextBody = $"Usa este enlace para restablecer tu contrasena: {resetUrl}"
            };

            var enviado = await SendAsync(toEmail, "Recuperacion de contrasena", builder, () =>
                _logger.LogWarning("SMTP no esta configurado. Enlace de recuperacion para {Email}: {ResetUrl}", toEmail, resetUrl));

            if (enviado)
            {
                _logger.LogInformation("Correo de recuperacion enviado por SMTP a {Email}", toEmail);
            }
        }

        // Construye y envia el correo con credenciales temporales emitidas por el administrador.
        public async Task SendNewUserCredentialsAsync(string toEmail, string userName, string temporaryPassword, string loginUrl)
        {
            var builder = new BodyBuilder
            {
                HtmlBody = $"""
                    <p>Tu usuario fue creado en Plantilla Base.</p>
                    <p><strong>Usuario:</strong> {userName}</p>
                    <p><strong>Contrasena temporal:</strong> {temporaryPassword}</p>
                    <p><a href="{loginUrl}">Iniciar sesion</a></p>
                    <p>Por seguridad, el sistema solicitara cambiar esta contrasena en el primer ingreso.</p>
                    """,
                TextBody = $"Tu usuario fue creado en Plantilla Base. Usuario: {userName}. Contrasena temporal: {temporaryPassword}. Ingresa en: {loginUrl}. El sistema solicitara cambiar esta contrasena en el primer ingreso."
            };

            var enviado = await SendAsync(toEmail, "Credenciales de acceso", builder, () =>
                _logger.LogWarning("SMTP no esta configurado. Credenciales temporales para {Email}. Usuario: {UserName}. Login: {LoginUrl}", toEmail, userName, loginUrl));

            if (enviado)
            {
                _logger.LogInformation("Correo de credenciales enviado por SMTP a {Email}", toEmail);
            }
        }

        // Envia un mensaje SMTP o ejecuta el fallback de log cuando no hay configuracion.
        private async Task<bool> SendAsync(string toEmail, string subject, BodyBuilder builder, Action logIfNotConfigured)
        {
            if (string.IsNullOrWhiteSpace(_settings.Host) ||
                string.IsNullOrWhiteSpace(_settings.UserName) ||
                string.IsNullOrWhiteSpace(_settings.Password) ||
                string.IsNullOrWhiteSpace(_settings.FromEmail))
            {
                logIfNotConfigured();
                return false;
            }

            // MimeMessage define remitente, destinatario, asunto y cuerpos HTML/texto.
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = builder.ToMessageBody();

            // Timeout corto para que una red bloqueada no deje esperando al usuario.
            using var client = new SmtpClient
            {
                Timeout = 15000
            };

            // Puerto 465 usa SSL desde el inicio; 587 usa STARTTLS.
            var socketOptions = _settings.Port == 465
                ? SecureSocketOptions.SslOnConnect
                : _settings.UseSsl
                    ? SecureSocketOptions.StartTls
                    : SecureSocketOptions.Auto;

            // Conecta, autentica con el proveedor SMTP y envia el mensaje.
            await client.ConnectAsync(_settings.Host, _settings.Port, socketOptions);
            await client.AuthenticateAsync(_settings.UserName, _settings.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }
    }
}
