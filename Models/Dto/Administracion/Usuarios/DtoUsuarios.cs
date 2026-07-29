using System.ComponentModel.DataAnnotations;

namespace Plantilla_Base.Models.Dto.Administracion.Usuarios
{
    // DTO recibido por POST /api/UsuariosApi/P_InsUsuario.
    // Contiene contrasena porque se usa solo al crear usuarios.
    public class DtoUsuarioCreateRequest
    {
        [Required(ErrorMessage = "La identificacion es obligatoria")]
        [StringLength(30, MinimumLength = 4)]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(60, MinimumLength = 3)]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        [StringLength(160)]
        public string E_Mail { get; set; } = string.Empty;

        [StringLength(128, MinimumLength = 10)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "La contrasena debe incluir mayuscula, minuscula y numero")]
        public string? Password { get; set; }
    }

    // DTO recibido por PUT /api/UsuariosApi/P_UdpUsuario/{id}.
    // No recibe contrasena. Los cambios de clave se atienden en un flujo
    // separado para exigir solicitud previa del usuario titular.
    public class DtoUsuarioUpdateRequest
    {
        [Required(ErrorMessage = "La identificacion es obligatoria")]
        [StringLength(30, MinimumLength = 4)]
        public string Identificacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120, MinimumLength = 2)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(30)]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        [StringLength(60, MinimumLength = 3)]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        [StringLength(160)]
        public string E_Mail { get; set; } = string.Empty;

        // Estado administrable desde el toggle de VwUsuarios.
        [Range(0, 1)]
        public short Vigente { get; set; } = 1;

        [Required(ErrorMessage = "El motivo de actualizacion es obligatorio")]
        [StringLength(4000, MinimumLength = 10, ErrorMessage = "El motivo debe tener entre 10 y 4000 caracteres")]
        public string Motivo_Actualiza { get; set; } = string.Empty;
    }

    // DTO recibido por POST /api/UsuariosApi/P_RestaurarContrasenaSolicitud/{id}.
    // La contrasena temporal es opcional: si el administrador la deja vacia,
    // el servicio genera una clave segura automaticamente.
    public class DtoUsuarioRestorePasswordRequest
    {
        [StringLength(128, MinimumLength = 10)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "La contrasena debe incluir mayuscula, minuscula y numero")]
        public string? Password { get; set; }
    }
}
