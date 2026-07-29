using System;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Base.Models.Administracion
{
    // Solicitud manual de restauracion de contrasena creada por el titular.
    // Permite que el administrador restaure una clave solo cuando existe una
    // solicitud pendiente y trazable.
    public class SolicitudRestaurarContrasena
    {
        [Key]
        public int Id_Solicitud_Restaura { get; set; }

        // Usuario titular que solicita ayuda al administrador.
        public int Id_Usuario { get; set; }

        [Required]
        [StringLength(30)]
        public string Identificacion_Solicitud { get; set; } = string.Empty;

        [Required]
        [StringLength(160)]
        public string Email_Solicitud { get; set; } = string.Empty;

        [Required]
        [StringLength(4000)]
        public string Motivo_Solicitud { get; set; } = string.Empty;

        // Pendiente, Usada, Rechazada o Vencida.
        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Pendiente";

        public short Vigente { get; set; } = 1;

        public DateTime Fecha_Solicitud { get; set; }

        public DateTime Fecha_Expiracion { get; set; }

        [StringLength(80)]
        public string? Ip_Solicitud { get; set; }

        [StringLength(500)]
        public string? UserAgent_Solicitud { get; set; }

        public DateTime? Fecha_Atencion { get; set; }

        public int? Id_Usuario_Atiende { get; set; }

        [StringLength(4000)]
        public string? Motivo_Atencion { get; set; }

        [StringLength(80)]
        public string? Ip_Atencion { get; set; }

        public Usuarios? Usuario { get; set; }
        public Usuarios? UsuarioAtiende { get; set; }
    }
}
