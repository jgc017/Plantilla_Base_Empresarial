namespace Plantilla_Base.Models.Dto.Account
{
    // Modelo de pagina para VwForgotPassword.
    // Mantiene separados el flujo automatico por enlace y la solicitud manual
    // al administrador para que cada formulario valide solo sus campos.
    public class DtoForgotPasswordPageViewModel
    {
        public DtoForgotPasswordViewModel Enlace { get; set; } = new();
        public DtoSolicitudRestaurarContrasenaViewModel SolicitudAdmin { get; set; } = new();
    }
}
