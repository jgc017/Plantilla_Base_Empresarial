using System.Threading.Tasks;

namespace Plantilla_Base.Services.Email
{
    // Contrato de envio de correos. Permite cambiar MailKit por otro proveedor
    // sin modificar AccountController.
    public interface IEmailSender
    {
        // Envia el enlace absoluto de recuperacion al correo del usuario.
        Task SendPasswordResetAsync(string toEmail, string resetUrl);

        // Envia credenciales temporales a usuarios creados por administrador.
        Task SendNewUserCredentialsAsync(string toEmail, string userName, string temporaryPassword, string loginUrl);
    }
}
