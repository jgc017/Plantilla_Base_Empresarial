using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.General;
using System.Security.Claims;

namespace Plantilla_Base.Controllers.Administracion
{
    // Controlador MVC de la pantalla de roles.
    // La vista VwRoles.cshtml consume el CRUD JSON expuesto por RolesApiController.
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IGeneral _general;

        public RolesController(IGeneral general)
        {
            _general = general;
        }

        // GET: /Roles/VwRoles
        // Renderiza la pantalla de registro/listado/edicion de roles.
        public async Task<IActionResult> VwRoles()
        {
            await _general.RegistrarAuditoria(GetAuditContext(), "VwRoles", "N/A", "Ingreso al formulario");
            return View();
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
