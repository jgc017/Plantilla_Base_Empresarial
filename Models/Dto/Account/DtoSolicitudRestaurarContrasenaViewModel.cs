using System.ComponentModel.DataAnnotations;

namespace Plantilla_Base.Models.Dto.Account
{
    // Modelo publico para que el titular solicite restauracion manual al administrador.
    public class DtoSolicitudRestaurarContrasenaViewModel
    {
        [Required(ErrorMessage = "La identificacion es obligatoria")]
        [StringLength(30, MinimumLength = 4)]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un email valido")]
        [StringLength(160)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(4000, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 4000 caracteres")]
        public string Motivo { get; set; } = string.Empty;
    }
}
