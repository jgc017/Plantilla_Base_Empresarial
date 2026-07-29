using System.ComponentModel.DataAnnotations;

namespace Plantilla_Base.Models.Dto.Administracion.SistemaConfig
{
    // DTO de lectura para pintar la configuracion visual actual del sistema.
    public class DtoSistemaVisualConfigItem
    {
        public int Id_SistemaVisualConfig { get; set; }
        public string LogoUrl { get; set; } = "/img/IMAGENIA.png";
        public string FaviconUrl { get; set; } = "/favicon.ico";
        public string LoginBackgroundUrl { get; set; } = "/img/auth-background.svg";
    }

    // DTO de actualizacion. Recibe las rutas publicas previamente cargadas en wwwroot.
    public class DtoSistemaVisualConfigUpdateRequest
    {
        [Required(ErrorMessage = "La ruta del logo es obligatoria.")]
        [StringLength(500, ErrorMessage = "La ruta del logo no puede superar 500 caracteres.")]
        public string LogoUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ruta del favicon es obligatoria.")]
        [StringLength(500, ErrorMessage = "La ruta del favicon no puede superar 500 caracteres.")]
        public string FaviconUrl { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ruta del fondo del login es obligatoria.")]
        [StringLength(500, ErrorMessage = "La ruta del fondo del login no puede superar 500 caracteres.")]
        public string LoginBackgroundUrl { get; set; } = string.Empty;
    }
}
