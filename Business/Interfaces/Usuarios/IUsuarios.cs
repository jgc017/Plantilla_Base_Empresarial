using Plantilla_Base.Business.Common;
using Plantilla_Base.Models.Dto.Administracion.Usuarios;
using System.Threading.Tasks;

namespace Plantilla_Base.Business.Interfaces.Usuarios
{
    // Contrato de negocio para el CRUD de usuarios.
    public interface IUsuarios
    {
        Task<bool> ExistenUsuarios();
        Task<ServiceResult> P_InsUsuario(DtoUsuarioCreateRequest model, AuditContext audit, bool esRegistroInicial, string? loginUrl);
        Task<ServiceResult> F_GetUsuariosList();
        Task<ServiceResult> F_GetUsuario(int idUsuario);
        Task<ServiceResult> F_GetUsuarioIdentificacion(string identificacion);
        Task<ServiceResult> P_UdpUsuario(int idUsuario, DtoUsuarioUpdateRequest model, AuditContext audit);
        Task<ServiceResult> P_RestaurarContrasenaSolicitud(int idUsuario, DtoUsuarioRestorePasswordRequest model, AuditContext audit, string? loginUrl);
        Task<ServiceResult> P_DeleteUsuario(int idUsuario, AuditContext audit);
    }
}
