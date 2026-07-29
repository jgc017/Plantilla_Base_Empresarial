using Plantilla_Base.Business.Common;
using Plantilla_Base.Models.Dto.Administracion.SistemaConfig;

namespace Plantilla_Base.Business.Interfaces.SistemaConfig
{
    // Contrato del flujo de configuracion visual global del sistema.
    public interface ISistemaConfig
    {
        Task<DtoSistemaVisualConfigItem> F_GetSistemaVisualConfig();
        Task<ServiceResult> P_UdpSistemaVisualConfig(DtoSistemaVisualConfigUpdateRequest model, AuditContext audit);
    }
}
