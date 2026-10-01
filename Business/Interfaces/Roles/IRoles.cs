using Plantilla_Base.Business.Common;
using Plantilla_Base.Models.Dto.Administracion.Roles;
using System.Threading.Tasks;

namespace Plantilla_Base.Business.Interfaces.Roles
{
    // Contrato de negocio para el CRUD de roles.
    public interface IRoles
    {
        Task<ServiceResult> P_InsRol(DtoRolCreateRequest model, AuditContext audit);
        Task<ServiceResult> F_GetRolesList();
        Task<ServiceResult> F_GetRol(int idRol);
        Task<ServiceResult> P_UdpRol(int idRol, DtoRolUpdateRequest model, AuditContext audit);
        Task<ServiceResult> P_DeleteRol(int idRol, string motivoElimina, AuditContext audit);
    }
}
