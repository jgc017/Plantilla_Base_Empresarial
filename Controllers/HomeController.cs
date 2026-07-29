using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plantilla_Base.Business.Interfaces.InicioAdmin;
using Plantilla_Base.Models.Dto.Administracion.InicioAdmin;
using Plantilla_Base.Models.Dto.General;
using System.Diagnostics;

namespace Plantilla_Base.Controllers
{
    // Controlador de inicio publico y paginas generales del proyecto.
    public class HomeController : Controller
    {
        private readonly IInicioAdmin _inicioAdmin;

        public HomeController(IInicioAdmin inicioAdmin)
        {
            _inicioAdmin = inicioAdmin;
        }

        // GET: /Home/VwIndex
        // Pagina publica inicial del sistema.
        [AllowAnonymous]
        public async Task<IActionResult> VwIndex()
        {
            return View(await _inicioAdmin.F_GetInicioPublico());
        }

        // GET: /Home/VwAcercaNosotros
        [AllowAnonymous]
        public async Task<IActionResult> VwAcercaNosotros()
        {
            var data = await _inicioAdmin.F_GetContenidoPublicoPorTipo(DtoInicioContenidoTipos.Acerca);
            return View(data);
        }

        // GET: /Home/VwNoticias
        [AllowAnonymous]
        public async Task<IActionResult> VwNoticias()
        {
            var data = await _inicioAdmin.F_GetContenidoPublicoPorTipo(DtoInicioContenidoTipos.Noticia);
            ViewData["Title"] = "Noticias";
            return View("VwContenidoLista", data);
        }

        // GET: /Home/VwPublicaciones
        [AllowAnonymous]
        public async Task<IActionResult> VwPublicaciones()
        {
            var data = await _inicioAdmin.F_GetContenidoPublicoPorTipo(DtoInicioContenidoTipos.Publicacion);
            ViewData["Title"] = "Publicaciones";
            return View("VwContenidoLista", data);
        }

        // GET: /Home/VwContenidoDetalle/{id}
        [AllowAnonymous]
        public async Task<IActionResult> VwContenidoDetalle(int id)
        {
            var data = await _inicioAdmin.F_GetContenidoPublicoDetalle(id);
            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // GET: /Home/VwContacto
        [AllowAnonymous]
        public async Task<IActionResult> VwContacto()
        {
            var data = await _inicioAdmin.F_GetContenidoPublicoPorTipo(DtoInicioContenidoTipos.Contacto);
            return View(data);
        }

        // GET: /Home/VwPrivacy
        // Vista publica de politicas de privacidad.
        [AllowAnonymous]
        public async Task<IActionResult> VwPrivacy()
        {
            var data = await _inicioAdmin.F_GetContenidoPublicoPorTipo(DtoInicioContenidoTipos.PoliticaPrivacidad);
            return View(data);
        }

        // GET: /Home/Error
        // Vista publica usada por el middleware de excepciones para mostrar
        // errores controlados sin exponer detalles internos.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View("VwError", new DtoErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
