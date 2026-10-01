using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.General;
using Plantilla_Base.Business.Interfaces.InicioAdmin;
using Plantilla_Base.Models.Dto.Administracion.InicioAdmin;
using System.Security.Claims;

namespace Plantilla_Base.Controllers.Api
{
    // API JSON usada por wwwroot/js/Administracion/InicioAdmin.js.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InicioAdminApiController : ControllerBase
    {
        private static readonly HashSet<string> ExtensionesImagenPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".gif"
        };

        private const long ImagenMaximaBytes = 5 * 1024 * 1024;
        private const int DominioTipoContenidoInicio = 10;
        private const int TipoContenidoSlider = 11;
        private const int SliderAnchoMinimo = 1440;
        private const int SliderAltoMinimo = 600;
        private const int ContenidoAnchoMinimo = 640;
        private const int ContenidoAltoMinimo = 360;
        private readonly IInicioAdmin _inicioAdmin;
        private readonly IGeneral _general;
        private readonly IWebHostEnvironment _environment;

        public InicioAdminApiController(IInicioAdmin inicioAdmin, IGeneral general, IWebHostEnvironment environment)
        {
            _inicioAdmin = inicioAdmin;
            _general = general;
            _environment = environment;
        }

        // POST: /api/InicioAdminApi/P_InsInicioContenido
        [HttpPost("P_InsInicioContenido")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_InsInicioContenido([FromBody] DtoInicioContenidoCreateRequest model)
        {
            if (!await TieneAccesoInicioAdmin())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return InvalidModelStateResponse();
            }

            var result = await _inicioAdmin.P_InsInicioContenido(model, GetAuditContext());
            await AuditarOperacion(result, "VwInicioAdmin", "P_InsInicioContenido");
            return ApiResponse(result);
        }

        // GET: /api/InicioAdminApi/F_GetInicioContenidosList
        // No audita porque alimenta la grilla administrativa.
        [HttpGet("F_GetInicioContenidosList")]
        public async Task<IActionResult> F_GetInicioContenidosList()
        {
            if (!await TieneAccesoInicioAdmin())
            {
                return Forbid();
            }

            return ApiResponse(await _inicioAdmin.F_GetInicioContenidosList());
        }

        // GET: /api/InicioAdminApi/F_GetInicioContenido/{id}
        [HttpGet("F_GetInicioContenido/{id}")]
        public async Task<IActionResult> F_GetInicioContenido(int id)
        {
            if (!await TieneAccesoInicioAdmin())
            {
                return Forbid();
            }

            var result = await _inicioAdmin.F_GetInicioContenido(id);
            await AuditarOperacion(result, "VwInicioAdmin", "F_GetInicioContenido");
            return ApiResponse(result);
        }

        // PUT: /api/InicioAdminApi/P_UdpInicioContenido/{id}
        [HttpPut("P_UdpInicioContenido/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_UdpInicioContenido(int id, [FromBody] DtoInicioContenidoUpdateRequest model)
        {
            if (!await TieneAccesoInicioAdmin())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return InvalidModelStateResponse();
            }

            var result = await _inicioAdmin.P_UdpInicioContenido(id, model, GetAuditContext());
            await AuditarOperacion(result, "VwInicioAdmin", "P_UdpInicioContenido");
            return ApiResponse(result);
        }

        // DELETE: /api/InicioAdminApi/P_DeleteInicioContenido/{id}
        [HttpDelete("P_DeleteInicioContenido/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_DeleteInicioContenido(int id, [FromQuery] string motivo)
        {
            if (!await TieneAccesoInicioAdmin())
            {
                return Forbid();
            }

            var result = await _inicioAdmin.P_DeleteInicioContenido(id, motivo, GetAuditContext());
            await AuditarOperacion(result, "VwInicioAdmin", "P_DeleteInicioContenido");
            return ApiResponse(result);
        }

        // POST: /api/InicioAdminApi/P_UploadImagenInicio
        // Guarda una imagen en wwwroot/img/inicio y retorna la ruta publica.
        [HttpPost("P_UploadImagenInicio")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_UploadImagenInicio(IFormFile imagen, [FromForm] int idTipoContenido)
        {
            if (!await TieneAccesoInicioAdmin())
            {
                return Forbid();
            }

            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest(new { ok = false, mensaje = "Debe seleccionar una imagen." });
            }

            if (!await TipoContenidoValido(idTipoContenido))
            {
                return BadRequest(new { ok = false, mensaje = "Debe seleccionar el tipo de contenido." });
            }

            if (imagen.Length > ImagenMaximaBytes)
            {
                return BadRequest(new { ok = false, mensaje = "La imagen no puede superar 5 MB." });
            }

            var extension = Path.GetExtension(imagen.FileName);
            if (!ExtensionesImagenPermitidas.Contains(extension))
            {
                return BadRequest(new { ok = false, mensaje = "Formato no permitido. Usa JPG, PNG, WEBP o GIF." });
            }

            var bytesImagen = await ImageMetadataReader.ObtenerBytesImagen(imagen);
            var dimensiones = ImageMetadataReader.ObtenerDimensionesImagen(bytesImagen, extension);
            if (dimensiones == null)
            {
                return BadRequest(new { ok = false, mensaje = "No fue posible leer las dimensiones de la imagen." });
            }

            var validacionDimensiones = ValidarDimensionesMinimas(idTipoContenido, dimensiones.Value.Width, dimensiones.Value.Height);
            if (validacionDimensiones != null)
            {
                return BadRequest(new { ok = false, mensaje = validacionDimensiones });
            }

            var carpetaDestino = Path.Combine(_environment.WebRootPath, "img", "inicio");
            Directory.CreateDirectory(carpetaDestino);

            var nombreArchivo = $"inicio_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{extension.ToLower()}";
            var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

            await System.IO.File.WriteAllBytesAsync(rutaFisica, bytesImagen);

            var rutaPublica = $"/img/inicio/{nombreArchivo}";
            await _general.RegistrarAuditoria(
                GetAuditContext(),
                "VwInicioAdmin",
                "P_UploadImagenInicio",
                $"Carga de imagen de inicio {rutaPublica}. Dimensiones: {dimensiones.Value.Width} x {dimensiones.Value.Height} px");

            return Ok(new
            {
                ok = true,
                mensaje = "Imagen cargada correctamente.",
                data = rutaPublica,
                dimensiones = new { ancho = dimensiones.Value.Width, alto = dimensiones.Value.Height }
            });
        }

        private static bool EsSlider(int idTipoContenido)
        {
            return idTipoContenido == TipoContenidoSlider;
        }

        private static string? ValidarDimensionesMinimas(int idTipoContenido, int ancho, int alto)
        {
            var esSlider = EsSlider(idTipoContenido);
            var anchoMinimo = esSlider ? SliderAnchoMinimo : ContenidoAnchoMinimo;
            var altoMinimo = esSlider ? SliderAltoMinimo : ContenidoAltoMinimo;
            var nombre = esSlider ? "slider" : "contenido";

            if (ancho < anchoMinimo || alto < altoMinimo)
            {
                return $"La imagen de {nombre} debe medir minimo {anchoMinimo} x {altoMinimo} px. Imagen seleccionada: {ancho} x {alto} px.";
            }

            return null;
        }

        private async Task<bool> TipoContenidoValido(int idTipoContenido)
        {
            if (idTipoContenido <= 0)
            {
                return false;
            }

            var dominios = await _general.ObtenerDominiosPorPadre(DominioTipoContenidoInicio);
            return dominios.Any(d => d.Id_Dominio == idTipoContenido);
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

        private List<int> GetCurrentUserRoles()
        {
            return User.FindAll("Id_Rol")
                .Select(c => int.TryParse(c.Value, out var idRol) ? idRol : 0)
                .Where(idRol => idRol > 0)
                .ToList();
        }

        private Task<bool> TieneAccesoInicioAdmin()
        {
            return _general.TienePermisoMenu(GetCurrentUserRoles(), "InicioAdmin", "VwInicioAdmin");
        }
    }
}

