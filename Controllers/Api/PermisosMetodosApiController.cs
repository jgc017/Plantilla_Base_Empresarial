using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.General;
using Plantilla_Base.Business.Interfaces.Permisos;
using Plantilla_Base.Models.Dto.Administracion.Permisos;
using System.Security.Claims;

namespace Plantilla_Base.Controllers.Api
{
    // API JSON usada por wwwroot/js/Administracion/PermisosMetodos.js.
    // Administra permisos automaticos generados desde metodos de controladores API.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PermisosMetodosApiController : ControllerBase
    {
        private readonly IPermisosMetodos _permisosMetodos;
        private readonly IGeneral _general;

        public PermisosMetodosApiController(IPermisosMetodos permisosMetodos, IGeneral general)
        {
            _permisosMetodos = permisosMetodos;
            _general = general;
        }

        // GET: /api/PermisosMetodosApi/F_GetPermisosMetodosList
        [HttpGet("F_GetPermisosMetodosList")]
        public async Task<IActionResult> F_GetPermisosMetodosList()
        {
            return ApiResponse(await _permisosMetodos.F_GetPermisosMetodosList());
        }

        // GET: /api/PermisosMetodosApi/F_GetPermisoMetodo/{id_Permiso}
        [HttpGet("F_GetPermisoMetodo/{id_Permiso}")]
        public async Task<IActionResult> F_GetPermisoMetodo(int id_Permiso)
        {
            var result = await _permisosMetodos.F_GetPermisoMetodo(id_Permiso);
            await AuditarOperacion(result, "VwPermisos", "F_GetPermisoMetodo");
            return ApiResponse(result);
        }

        // POST: /api/PermisosMetodosApi/P_SyncPermisosMetodos
        [HttpPost("P_SyncPermisosMetodos")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_SyncPermisosMetodos()
        {
            var result = await _permisosMetodos.P_SyncPermisosMetodos(GetAuditContext());
            await AuditarOperacion(result, "VwPermisos", "P_SyncPermisosMetodos");
            return ApiResponse(result);
        }

        // PUT: /api/PermisosMetodosApi/P_UdpPermisoMetodo/{id_Permiso}
        [HttpPut("P_UdpPermisoMetodo/{id_Permiso}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_UdpPermisoMetodo(int id_Permiso, [FromBody] DtoPermisoMetodoUpdateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return InvalidModelStateResponse();
            }

            var result = await _permisosMetodos.P_UdpPermisoMetodo(id_Permiso, model, GetAuditContext());
            await AuditarOperacion(result, "VwPermisos", "P_UdpPermisoMetodo");
            return ApiResponse(result);
        }

        // DELETE: /api/PermisosMetodosApi/P_DeletePermisoMetodo/{id_Permiso}
        [HttpDelete("P_DeletePermisoMetodo/{id_Permiso}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_DeletePermisoMetodo(int id_Permiso, [FromQuery] string motivo)
        {
            var result = await _permisosMetodos.P_DeletePermisoMetodo(id_Permiso, motivo, GetAuditContext());
            await AuditarOperacion(result, "VwPermisos", "P_DeletePermisoMetodo");
            return ApiResponse(result);
        }

        private IActionResult ApiResponse(ServiceResult result)
        {
            return StatusCode(result.StatusCode, result.ToApiResponse());
        }

        private async Task AuditarOperacion(ServiceResult result, string formulario, string metodoEjecutado)
        {
            if (result.Ok && !string.IsNullOrWhiteSpace(result.AuditDescription))
            {
                await _general.RegistrarAuditoria(GetAuditContext(), formulario, metodoEjecutado, result.AuditDescription);
            }
        }

        private BadRequestObjectResult InvalidModelStateResponse()
        {
            return BadRequest(new
            {
                ok = false,
                mensaje = "Datos invalidos",
                errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
            });
        }

        private AuditContext GetAuditContext()
        {
            return new AuditContext(GetCurrentUserId(), HttpContext.Connection.RemoteIpAddress?.ToString());
        }

        private int? GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(id, out var usuarioId) ? usuarioId : null;
        }
    }
}

