using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.SistemaConfig;
using Plantilla_Base.Data;
using Plantilla_Base.Models.Administracion;
using Plantilla_Base.Models.Dto.Administracion.SistemaConfig;

namespace Plantilla_Base.Business.Services.SistemaConfig
{
    // Servicio de negocio para las imagenes globales del sistema.
    // Mantiene una sola configuracion activa y entrega valores por defecto si aun no existe tabla o registro.
    public class SistemaConfigService : ISistemaConfig
    {
        private const string LogoDefault = "/img/IMAGENIA.png";
        private const string FaviconDefault = "/favicon.ico";
        private const string LoginBackgroundDefault = "/img/auth-background.svg";
        private readonly AppDbContext _context;
        private readonly ILogger<SistemaConfigService> _logger;

        public SistemaConfigService(AppDbContext context, ILogger<SistemaConfigService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // F_GetSistemaVisualConfig: obtiene la configuracion vigente o retorna defaults.
        public async Task<DtoSistemaVisualConfigItem> F_GetSistemaVisualConfig()
        {
            try
            {
                var config = await _context.SistemaVisualConfig
                    .AsNoTracking()
                    .Where(c => c.Vigente == 1)
                    .OrderBy(c => c.Id_SistemaVisualConfig)
                    .Select(c => new DtoSistemaVisualConfigItem
                    {
                        Id_SistemaVisualConfig = c.Id_SistemaVisualConfig,
                        LogoUrl = c.LogoUrl,
                        FaviconUrl = c.FaviconUrl,
                        LoginBackgroundUrl = c.LoginBackgroundUrl
                    })
                    .FirstOrDefaultAsync();

                return config ?? ObtenerConfigDefault();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No fue posible leer SistemaVisualConfig. Se usaran imagenes por defecto.");
                return ObtenerConfigDefault();
            }
        }

        // P_UdpSistemaVisualConfig: crea o actualiza la configuracion visual global.
        public async Task<ServiceResult> P_UdpSistemaVisualConfig(DtoSistemaVisualConfigUpdateRequest model, AuditContext audit)
        {
            var logo = NormalizarRuta(model.LogoUrl);
            var favicon = NormalizarRuta(model.FaviconUrl);
            var loginBackground = NormalizarRuta(model.LoginBackgroundUrl);

            if (!RutaLocalValida(logo) || !RutaLocalValida(favicon) || !RutaLocalValida(loginBackground))
            {
                return ServiceResult.Fail(StatusCodes.Status400BadRequest, "Las rutas deben ser locales y comenzar por /img/ o /favicon.ico.");
            }

            var config = await _context.SistemaVisualConfig
                .Where(c => c.Vigente == 1)
                .OrderBy(c => c.Id_SistemaVisualConfig)
                .FirstOrDefaultAsync();

            if (config == null)
            {
                config = new SistemaVisualConfig
                {
                    LogoUrl = logo,
                    FaviconUrl = favicon,
                    LoginBackgroundUrl = loginBackground,
                    Vigente = 1,
                    Id_Usuario_Creacion = audit.UserId,
                    Fecha_Creacion = DateTime.UtcNow,
                    Maquina_Creacion = audit.Machine
                };

                _context.SistemaVisualConfig.Add(config);
            }
            else
            {
                config.LogoUrl = logo;
                config.FaviconUrl = favicon;
                config.LoginBackgroundUrl = loginBackground;
                config.Id_Usuario_Modifica = audit.UserId;
                config.Fecha_Modifica = DateTime.UtcNow;
                config.Maquina_Modifica = audit.Machine;
            }

            await _context.SaveChangesAsync();

            return ServiceResult.Success(
                "Configuracion visual actualizada correctamente.",
                await F_GetSistemaVisualConfig(),
                auditDescription: "Actualizacion de logo, favicon y fondo de login del sistema");
        }

        private static DtoSistemaVisualConfigItem ObtenerConfigDefault()
        {
            return new DtoSistemaVisualConfigItem
            {
                LogoUrl = LogoDefault,
                FaviconUrl = FaviconDefault,
                LoginBackgroundUrl = LoginBackgroundDefault
            };
        }

        private static string NormalizarRuta(string ruta)
        {
            var value = ruta.Trim();
            return value.StartsWith("~/", StringComparison.Ordinal) ? value[1..] : value;
        }

        private static bool RutaLocalValida(string ruta)
        {
            return ruta.StartsWith("/img/", StringComparison.OrdinalIgnoreCase)
                || string.Equals(ruta, "/favicon.ico", StringComparison.OrdinalIgnoreCase);
        }
    }
}
