using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.General;
using Plantilla_Base.Business.Interfaces.SistemaConfig;
using Plantilla_Base.Models.Dto.Administracion.SistemaConfig;
using System.Security.Claims;

namespace Plantilla_Base.Controllers.Api
{
    // API JSON usada por wwwroot/js/Administracion/SistemaConfig.js.
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SistemaConfigApiController : ControllerBase
    {
        private static readonly HashSet<string> ExtensionesLogoPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp",
            ".gif"
        };

        private static readonly HashSet<string> ExtensionesFaviconPermitidas = new(StringComparer.OrdinalIgnoreCase)
        {
            ".ico",
            ".png"
        };

        private const long ImagenMaximaBytes = 5 * 1024 * 1024;
        private const int LogoAnchoMinimo = 160;
        private const int LogoAltoMinimo = 80;
        private const int FaviconAnchoMinimo = 32;
        private const int FaviconAltoMinimo = 32;
        private const int FondoLoginAnchoMinimo = 1280;
        private const int FondoLoginAltoMinimo = 720;
        private readonly ISistemaConfig _sistemaConfig;
        private readonly IGeneral _general;
        private readonly IWebHostEnvironment _environment;

        public SistemaConfigApiController(ISistemaConfig sistemaConfig, IGeneral general, IWebHostEnvironment environment)
        {
            _sistemaConfig = sistemaConfig;
            _general = general;
            _environment = environment;
        }

        // GET: /api/SistemaConfigApi/F_GetSistemaVisualConfig
        [HttpGet("F_GetSistemaVisualConfig")]
        public async Task<IActionResult> F_GetSistemaVisualConfig()
        {
            if (!await TieneAccesoSistemaConfig())
            {
                return Forbid();
            }

            return Ok(new
            {
                ok = true,
                data = await _sistemaConfig.F_GetSistemaVisualConfig()
            });
        }

        // PUT: /api/SistemaConfigApi/P_UdpSistemaVisualConfig
        [HttpPut("P_UdpSistemaVisualConfig")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_UdpSistemaVisualConfig([FromBody] DtoSistemaVisualConfigUpdateRequest model)
        {
            if (!await TieneAccesoSistemaConfig())
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return InvalidModelStateResponse();
            }

            var result = await _sistemaConfig.P_UdpSistemaVisualConfig(model, GetAuditContext());
            if (result.Ok && !string.IsNullOrWhiteSpace(result.AuditDescription))
            {
                await _general.RegistrarAuditoria(GetAuditContext(), "VwSistemaConfig", "P_UdpSistemaVisualConfig", result.AuditDescription);
            }

            return StatusCode(result.StatusCode, result.ToApiResponse());
        }

        // POST: /api/SistemaConfigApi/P_UploadImagenSistema
        // Guarda una imagen en wwwroot/img/sistema y devuelve la ruta publica para el formulario.
        [HttpPost("P_UploadImagenSistema")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> P_UploadImagenSistema(IFormFile imagen, [FromForm] string tipoImagen)
        {
            if (!await TieneAccesoSistemaConfig())
            {
                return Forbid();
            }

            if (imagen == null || imagen.Length == 0)
            {
                return BadRequest(new { ok = false, mensaje = "Debe seleccionar una imagen." });
            }

            if (imagen.Length > ImagenMaximaBytes)
            {
                return BadRequest(new { ok = false, mensaje = "La imagen no puede superar 5 MB." });
            }

            var tipo = (tipoImagen ?? string.Empty).Trim().ToLowerInvariant();
            if (tipo is not ("logo" or "favicon" or "loginbackground"))
            {
                return BadRequest(new { ok = false, mensaje = "Tipo de imagen no valido." });
            }

            var extension = Path.GetExtension(imagen.FileName);
            var extensionesPermitidas = tipo == "favicon" ? ExtensionesFaviconPermitidas : ExtensionesLogoPermitidas;
            if (!extensionesPermitidas.Contains(extension))
            {
                var formatos = tipo == "favicon" ? "ICO o PNG" : "JPG, PNG, WEBP o GIF";
                return BadRequest(new { ok = false, mensaje = $"Formato no permitido. Usa {formatos}." });
            }

            var bytesImagen = await ImageMetadataReader.ObtenerBytesImagen(imagen);
            var dimensiones = ImageMetadataReader.ObtenerDimensionesImagen(bytesImagen, extension);
            if (dimensiones == null)
            {
                return BadRequest(new { ok = false, mensaje = "No fue posible leer las dimensiones de la imagen." });
            }

            var validacionDimensiones = ValidarDimensionesMinimas(tipo, dimensiones.Value.Width, dimensiones.Value.Height);
            if (validacionDimensiones != null)
            {
                return BadRequest(new { ok = false, mensaje = validacionDimensiones });
            }

            var carpetaDestino = Path.Combine(_environment.WebRootPath, "img", "sistema");
            Directory.CreateDirectory(carpetaDestino);

            var nombreArchivo = $"{tipo}_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
            var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

            await System.IO.File.WriteAllBytesAsync(rutaFisica, bytesImagen);

            var rutaPublica = $"/img/sistema/{nombreArchivo}";
            await _general.RegistrarAuditoria(
                GetAuditContext(),
                "VwSistemaConfig",
                "P_UploadImagenSistema",
                $"Carga de imagen visual {tipo}: {rutaPublica}. Dimensiones: {dimensiones.Value.Width} x {dimensiones.Value.Height} px");

            return Ok(new
            {
                ok = true,
                mensaje = "Imagen cargada correctamente.",
                data = rutaPublica,
                dimensiones = new { ancho = dimensiones.Value.Width, alto = dimensiones.Value.Height }
            });
        }

        private static string? ValidarDimensionesMinimas(string tipo, int ancho, int alto)
        {
            var (anchoMinimo, altoMinimo, nombre) = tipo switch
            {
                "logo" => (LogoAnchoMinimo, LogoAltoMinimo, "logo del sistema"),
                "favicon" => (FaviconAnchoMinimo, FaviconAltoMinimo, "favicon"),
                "loginbackground" => (FondoLoginAnchoMinimo, FondoLoginAltoMinimo, "fondo del login"),
                _ => (0, 0, "imagen")
            };

            if (ancho < anchoMinimo || alto < altoMinimo)
            {
                return $"La imagen para {nombre} debe medir minimo {anchoMinimo} x {altoMinimo} px. Imagen seleccionada: {ancho} x {alto} px.";
            }

            return null;
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

        private Task<bool> TieneAccesoSistemaConfig()
        {
            return _general.TienePermisoMenu(GetCurrentUserRoles(), "SistemaConfig", "VwSistemaConfig");
        }
    }
}
