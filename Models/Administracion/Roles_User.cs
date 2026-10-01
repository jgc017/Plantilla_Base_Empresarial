using System;
using System.ComponentModel.DataAnnotations;

namespace Plantilla_Base.Models.Administracion
{
    // Entidad puente entre Usuarios y Roles.
    // Permite asignar varios roles a un mismo usuario.
    public class Roles_User
    {
        [Key]
        public int Id_Roles_User { get; set; }

        // Llaves foraneas configuradas en AppDbContext.
        public int Id_Usuario { get; set; }
        public int Id_Rol { get; set; }

        // Baja logica de la asignacion.
        public short Vigente { get; set; } = 1;

                [StringLength(4000)]
        public string? Motivo_Actualiza { get; set; }

        [StringLength(4000)]
        public string? Motivo_Elimina { get; set; }

        // Auditoria de creacion.
        public int? Id_Usuario_Creacion { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public string? Maquina_Creacion { get; set; }

        // Auditoria de modificacion.
        public int? Id_Usuario_Modifica { get; set; }
        public DateTime? Fecha_Modifica { get; set; }
        public string? Maquina_Modifica { get; set; }

        // Propiedades de navegacion para consultar datos relacionados con EF.
        public Usuarios Usuario { get; set; } = null!;
        public Roles Rol { get; set; } = null!;
    }

}

