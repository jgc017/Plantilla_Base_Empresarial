using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.General;
using System.Security.Claims;

namespace Plantilla_Base.Controllers.Administracion
{
    // Controlador MVC de la pantalla para administrar el inicio publico.
    [Authorize]
    public class InicioAdminController : Controller
    {
        private const int DominioTipoContenidoInicio = 10;
        private readonly IGeneral _general;

        public InicioAdminController(IGeneral general)
        {
            _general = general;
        }

        // GET: /InicioAdmin/VwInicioAdmin
        // Renderiza la pantalla unica de administracion del inicio y sus paginas publicas.
        public async Task<IActionResult> VwInicioAdmin()
        {
            if (!await _general.TienePermisoMenu(GetCurrentUserRoles(), "InicioAdmin", "VwInicioAdmin"))
            {
                return Forbid();
            }

            ViewBag.ddlTiposContenido = new SelectList(
                await _general.ObtenerDominiosPorPadre(DominioTipoContenidoInicio),
                "Id_Dominio",
                "Descripcion");

            await _general.RegistrarAuditoria(GetAuditContext(), "VwInicioAdmin", "N/A", "Ingreso al formulario");
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

        private List<int> GetCurrentUserRoles()
        {
            return User.FindAll("Id_Rol")
                .Select(c => int.TryParse(c.Value, out var idRol) ? idRol : 0)
                .Where(idRol => idRol > 0)
                .ToList();
        }
    }
}
