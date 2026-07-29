using System.ComponentModel.DataAnnotations;

namespace Plantilla_Base.Models.Dto.Administracion.InicioAdmin
{
    // Constantes de tipos de contenido administrables para el inicio publico.
    public static class DtoInicioContenidoTipos
    {
        public const string Slider = "Slider";
        public const string Acerca = "Acerca";
        public const string Noticia = "Noticia";
        public const string Publicacion = "Publicacion";
        public const string Contacto = "Contacto";
        public const string PoliticaPrivacidad = "PoliticaPrivacidad";

        public static readonly string[] Permitidos =
        {
            Slider,
            Acerca,
            Noticia,
            Publicacion,
            Contacto,
            PoliticaPrivacidad
        };
    }

    // DTO recibido por POST /api/InicioAdminApi/P_InsInicioContenido.
    public class DtoInicioContenidoCreateRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "El tipo de contenido es obligatorio")]
        public int IdTipoContenido { get; set; }

        [Required(ErrorMessage = "El tipo de contenido es obligatorio")]
        [StringLength(40)]
        public string TipoContenido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El titulo es obligatorio")]
        [StringLength(160, MinimumLength = 2)]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Resumen { get; set; }

        public string? Contenido { get; set; }

        [StringLength(500)]
        public string? ImagenUrl { get; set; }

        [StringLength(500)]
        public string? EnlaceUrl { get; set; }

        [StringLength(80)]
        public string? TextoBoton { get; set; }

        [Range(0, 1)]
        public short MostrarEnInicio { get; set; } = 1;

        [Range(0, int.MaxValue)]
        public int Orden { get; set; }
    }

    // DTO recibido por PUT /api/InicioAdminApi/P_UdpInicioContenido/{id}.
    public class DtoInicioContenidoUpdateRequest : DtoInicioContenidoCreateRequest
    {
        [Range(0, 1)]
        public short Vigente { get; set; } = 1;
    }

    // DTO de lectura usado por la vista publica y administrativa.
    public class DtoInicioContenidoItem
    {
        public int Id_InicioContenido { get; set; }
        public string TipoContenido { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Resumen { get; set; }
        public string? Contenido { get; set; }
        public string? ImagenUrl { get; set; }
        public string? EnlaceUrl { get; set; }
        public string? TextoBoton { get; set; }
        public short MostrarEnInicio { get; set; }
        public int Orden { get; set; }
        public short Vigente { get; set; }
        public DateTime Fecha_Creacion { get; set; }
    }

    // DTO agregado para pintar la pagina Home/VwIndex.
    public class DtoInicioPublico
    {
        public List<DtoInicioContenidoItem> Slider { get; set; } = new();
        public List<DtoInicioContenidoItem> Noticias { get; set; } = new();
        public List<DtoInicioContenidoItem> Publicaciones { get; set; } = new();
        public DtoInicioContenidoItem? Acerca { get; set; }
        public DtoInicioContenidoItem? Contacto { get; set; }
    }
}
