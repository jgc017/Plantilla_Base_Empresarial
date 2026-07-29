using Plantilla_Base.Business.Common;
using Plantilla_Base.Models.Dto.Administracion.RolesUser;
using System.Threading.Tasks;

namespace Plantilla_Base.Business.Interfaces.RolesUser
{
    // Contrato de negocio para consultar y sincronizar roles asignados a usuarios.
    public interface IRolesUser
    {
        Task<ServiceResult> GetIdUserRoles(int idUsuario);
        Task<ServiceResult> Asignar(int idUsuario, DtoRolesUserUpdateRequest model, AuditContext audit);
    }
}
