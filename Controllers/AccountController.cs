using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Plantilla_Base.Data;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.General;
using Plantilla_Base.Business.Interfaces.Usuarios;
using Plantilla_Base.Models.Account;
using Plantilla_Base.Models.Administracion;
using Plantilla_Base.Models.Dto.Account;
using Plantilla_Base.Models.Dto.Administracion.Usuarios;
using Plantilla_Base.Services.Email;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Plantilla_Base.Controllers
{
    // Controlador responsable del flujo de autenticacion:
    // login, logout, olvido de contrasena, restablecimiento y acceso denegado.
    public class AccountController : Controller
    {
        // Claim propio para guardar los Id_Rol asignados al usuario autenticado.
        // Permite validar permisos por identificador estable, no por descripcion del rol.
        public const string RoleIdClaimType = "Id_Rol";
        public const string MustChangePasswordClaimType = "Debe_Cambiar_Password";

        private readonly AppDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IGeneral _general;
        private readonly IUsuarios _usuarios;
        private readonly ILogger<AccountController> _logger;

        // Recibe el DbContext para consultar usuarios/tokens, el servicio de
        // correo para enviar recuperaciones y el logger para diagnostico.
        public AccountController(
            AppDbContext context,
            IEmailSender emailSender,
            IGeneral general,
            IUsuarios usuarios,
            ILogger<AccountController> logger)
        {
            _context = context;
            _emailSender = emailSender;
            _general = general;
            _usuarios = usuarios;
            _logger = logger;
        }

        // GET: /Account/Login
        // Muestra el formulario de inicio de sesion. Si ya hay sesion activa,
        // redirige al destino solicitado o al Home.
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToLocal(returnUrl);
            }

            ViewBag.PermiteRegistroInicial = !await _usuarios.ExistenUsuarios();
            return View("VwLogin", new DtoLoginViewModel { ReturnUrl = returnUrl });
        }

        // GET: /Account/RegistroInicial
        // Permite crear el primer usuario unicamente cuando la tabla Usuarios esta vacia.
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> RegistroInicial()
        {
            if (await _usuarios.ExistenUsuarios())
            {
                return RedirectToAction(nameof(Login));
            }

            return View("VwInitialRegister", new DtoInitialRegisterViewModel());
        }

        // POST: /Account/RegistroInicial
        // Crea el primer usuario del sistema y, si existe el rol 1, lo deja como super usuario.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> RegistroInicial(DtoInitialRegisterViewModel model)
        {
            if (await _usuarios.ExistenUsuarios())
            {
                return RedirectToAction(nameof(Login));
            }

            if (!ModelState.IsValid)
            {
                return View("VwInitialRegister", model);
            }

            var request = new DtoUsuarioCreateRequest
            {
                Identificacion = model.Identificacion,
                Nombre = model.Nombre,
                Telefono = model.Telefono,
                Usuario = model.Usuario,
                E_Mail = model.E_Mail,
                Password = model.Password
            };

            var loginUrl = Url.Action(nameof(Login), "Account", null, Request.Scheme);
            var result = await _usuarios.P_InsUsuario(request, GetAuditContext(), esRegistroInicial: true, loginUrl);

            if (!result.Ok)
            {
                ModelState.AddModelError(string.Empty, result.Mensaje ?? "No fue posible crear el primer usuario.");
                return View("VwInitialRegister", model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Usuario.ToLower() == model.Usuario.Trim().ToLowerInvariant());

            if (usuario != null)
            {
                await SignInUserAsync(usuario);
            }

            return RedirectToAction("VwIndex", "Home");
        }

        // POST: /Account/Login
        // Valida usuario/email y contrasena, crea los claims y emite la cookie
        // de autenticacion usada por el resto del proyecto.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login(DtoLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.PermiteRegistroInicial = !await _usuarios.ExistenUsuarios();
                return View("VwLogin", model);
            }

            // Permite iniciar sesion usando nombre de usuario o correo.
            var login = model.Usuario.Trim().ToLowerInvariant();
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u =>
                    u.Vigente == 1 &&
                    (u.Usuario.ToLower() == login || (u.E_Mail != null && u.E_Mail.ToLower() == login)));

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
            {
                _logger.LogWarning("Intento de login fallido para {Usuario}", model.Usuario);
                ModelState.AddModelError(string.Empty, "Usuario o contrasena incorrectos.");
                ViewBag.PermiteRegistroInicial = !await _usuarios.ExistenUsuarios();
                return View("VwLogin", model);
            }

            await SignInUserAsync(usuario);

            if (usuario.Debe_Cambiar_Password == 1)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            return RedirectToLocal(model.ReturnUrl);
        }

        // GET: /Account/ChangePassword
        // Pantalla obligatoria para usuarios que ingresan con contrasena temporal.
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("VwChangePassword", new DtoChangePasswordViewModel());
        }

        // POST: /Account/ChangePassword
        // Cambia la contrasena temporal y libera al usuario para navegar el sistema.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> ChangePassword(DtoChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VwChangePassword", model);
            }

            var usuarioId = GetCurrentUserId();
            if (!usuarioId.HasValue)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(Login));
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id_Usuario == usuarioId.Value && u.Vigente == 1);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.CurrentPassword, usuario.Password))
            {
                ModelState.AddModelError(string.Empty, "La contrasena actual no es correcta.");
                return View("VwChangePassword", model);
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, workFactor: 12);
            usuario.Debe_Cambiar_Password = 0;
            usuario.Id_Usuario_Modifica = usuario.Id_Usuario;
            usuario.Fecha_Modifica = DateTime.UtcNow;
            usuario.Maquina_Modifica = GetClientIp();

            await _context.SaveChangesAsync();
            await SignInUserAsync(usuario);

            return RedirectToAction("VwIndex", "Home");
        }

        // Crea la cookie de autenticacion con los datos actuales del usuario y sus roles vigentes.
        private async Task SignInUserAsync(Usuarios usuario)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id_Usuario.ToString()),
                new(ClaimTypes.Name, usuario.Usuario),
                new(ClaimTypes.Email, usuario.E_Mail ?? string.Empty)
            };

            if (usuario.Debe_Cambiar_Password == 1)
            {
                claims.Add(new Claim(MustChangePasswordClaimType, "1"));
            }

            // Agrega roles vigentes del usuario a la cookie:
            // - ClaimTypes.Role conserva el nombre para compatibilidad con [Authorize(Roles=...)].
            // - RoleIdClaimType conserva el Id_Rol para validaciones internas mas estables.
            var roles = await (from ru in _context.Roles_User.AsNoTracking()
                               join rol in _context.Roles.AsNoTracking() on ru.Id_Rol equals rol.Id_Rol
                               where ru.Id_Usuario == usuario.Id_Usuario && ru.Vigente == 1 && rol.Vigente == 1
                               select new
                               {
                                   rol.Id_Rol,
                                   rol.Rol
                               })
                .ToListAsync();

            claims.AddRange(roles.Select(rol => new Claim(ClaimTypes.Role, rol.Rol)));
            claims.AddRange(roles.Select(rol => new Claim(RoleIdClaimType, rol.Id_Rol.ToString())));

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    IssuedUtc = DateTimeOffset.UtcNow
                });
        }

        // GET: /Account/ForgotPassword
        // Muestra el formulario donde el usuario ingresa su correo.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("VwForgotPassword", new DtoForgotPasswordPageViewModel());
        }

        // POST: /Account/ForgotPassword
        // Genera un token de recuperacion y envia el enlace por correo.
        // Siempre responde con pantalla de confirmacion para no revelar si el
        // correo existe o no en la base de datos.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> ForgotPassword([Bind(Prefix = "Enlace")] DtoForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VwForgotPassword", new DtoForgotPasswordPageViewModel { Enlace = model });
            }

            var email = model.Email.Trim().ToLowerInvariant();
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Vigente == 1 && u.E_Mail != null && u.E_Mail.ToLower() == email);

            if (usuario != null)
            {
                _logger.LogInformation("Solicitud de recuperacion valida para el usuario {UsuarioId}", usuario.Id_Usuario);

                // Invalida tokens anteriores aun vigentes para que solo el
                // ultimo enlace de recuperacion sea util.
                var now = DateTime.UtcNow;
                var tokensPendientes = await _context.PasswordResetTokens
                    .Where(t => t.Id_Usuario == usuario.Id_Usuario &&
                                t.Fecha_Uso == null &&
                                t.Fecha_Expiracion > now)
                    .ToListAsync();

                foreach (var tokenPendiente in tokensPendientes)
                {
                    tokenPendiente.Fecha_Uso = now;
                }

                var token = CreateToken();

                // Solo se almacena el hash del token. El token real viaja una
                // sola vez en el enlace enviado al correo.
                _context.PasswordResetTokens.Add(new PasswordResetToken
                {
                    Id_Usuario = usuario.Id_Usuario,
                    TokenHash = HashToken(token),
                    Fecha_Creacion = now,
                    Fecha_Expiracion = now.AddMinutes(30),
                    Ip_Solicitud = GetClientIp()
                });

                await _context.SaveChangesAsync();

                // Url.Action construye el enlace absoluto al GET ResetPassword.
                var resetUrl = Url.Action(
                    nameof(ResetPassword),
                    "Account",
                    new { token },
                    Request.Scheme);

                if (!string.IsNullOrWhiteSpace(resetUrl))
                {
                    try
                    {
                        await _emailSender.SendPasswordResetAsync(usuario.E_Mail!, resetUrl);
                        _logger.LogInformation("Correo de recuperacion procesado para el usuario {UsuarioId}", usuario.Id_Usuario);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "No fue posible enviar el correo de recuperacion para el usuario {UsuarioId}", usuario.Id_Usuario);
                    }
                }
            }
            else
            {
                _logger.LogInformation("Solicitud de recuperacion recibida para un correo no registrado o usuario inactivo");
            }

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        // POST: /Account/SolicitarRestauracionAdministrador
        // Registra una solicitud manual validando identificacion + correo.
        // La respuesta sigue siendo generica para evitar enumeracion de cuentas.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> SolicitarRestauracionAdministrador(
            [Bind(Prefix = "SolicitudAdmin")] DtoSolicitudRestaurarContrasenaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VwForgotPassword", new DtoForgotPasswordPageViewModel { SolicitudAdmin = model });
            }

            var identificacion = model.Identificacion.Trim();
            var email = model.Email.Trim().ToLowerInvariant();
            var now = DateTime.UtcNow;

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.Vigente == 1 &&
                    u.Identificacion == identificacion &&
                    u.E_Mail != null &&
                    u.E_Mail.ToLower() == email);

            if (usuario != null)
            {
                var solicitudesExpiradas = await _context.Solicitud_Restaurar_Contrasena
                    .Where(s => s.Id_Usuario == usuario.Id_Usuario &&
                                s.Estado == "Pendiente" &&
                                s.Vigente == 1 &&
                                s.Fecha_Expiracion <= now)
                    .ToListAsync();

                foreach (var solicitudExpirada in solicitudesExpiradas)
                {
                    solicitudExpirada.Estado = "Vencida";
                    solicitudExpirada.Vigente = 0;
                }

                var existeSolicitudPendiente = await _context.Solicitud_Restaurar_Contrasena
                    .AnyAsync(s => s.Id_Usuario == usuario.Id_Usuario &&
                                   s.Estado == "Pendiente" &&
                                   s.Vigente == 1 &&
                                   s.Fecha_Expiracion > now);

                if (!existeSolicitudPendiente)
                {
                    _context.Solicitud_Restaurar_Contrasena.Add(new SolicitudRestaurarContrasena
                    {
                        Id_Usuario = usuario.Id_Usuario,
                        Identificacion_Solicitud = identificacion,
                        Email_Solicitud = email,
                        Motivo_Solicitud = model.Motivo.Trim(),
                        Estado = "Pendiente",
                        Vigente = 1,
                        Fecha_Solicitud = now,
                        Fecha_Expiracion = now.AddHours(24),
                        Ip_Solicitud = GetClientIp(),
                        UserAgent_Solicitud = Request.Headers.UserAgent.ToString()
                    });
                }

                await _context.SaveChangesAsync();

                await _general.RegistrarAuditoria(
                    new AuditContext(usuario.Id_Usuario, GetClientIp()),
                    "VwForgotPassword",
                    "P_SolicitarRestauracionAdministrador",
                    $"Solicitud manual de restauracion de contrasena para el usuario {usuario.Usuario} con id {usuario.Id_Usuario}");
            }
            else
            {
                _logger.LogInformation("Solicitud manual de restauracion no asociada a usuario activo");
            }

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        // GET: /Account/ForgotPasswordConfirmation
        // Confirmacion generica despues de solicitar recuperacion.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View("VwForgotPasswordConfirmation");
        }

        // GET: /Account/ResetPassword?token=...
        // Muestra el formulario para escribir una nueva contrasena.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string? token = null)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(nameof(Login));
            }

            return View("VwResetPassword", new DtoResetPasswordViewModel { Token = token });
        }

        // POST: /Account/ResetPassword
        // Valida el token, actualiza la contrasena con BCrypt y marca el token
        // como usado para impedir reutilizacion.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> ResetPassword(DtoResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("VwResetPassword", model);
            }

            var tokenHash = HashToken(model.Token);
            var now = DateTime.UtcNow;

            var token = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t =>
                    t.TokenHash == tokenHash &&
                    t.Fecha_Uso == null &&
                    t.Fecha_Expiracion > now);

            if (token == null)
            {
                ModelState.AddModelError(string.Empty, "El enlace de recuperacion no es valido o ya expiro.");
                return View("VwResetPassword", model);
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id_Usuario == token.Id_Usuario && u.Vigente == 1);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "El enlace de recuperacion no es valido o ya expiro.");
                return View("VwResetPassword", model);
            }

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, workFactor: 12);
            usuario.Debe_Cambiar_Password = 0;
            usuario.Fecha_Modifica = now;
            usuario.Maquina_Modifica = GetClientIp();
            token.Fecha_Uso = now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        // GET: /Account/ResetPasswordConfirmation
        // Pantalla final luego de cambiar la contrasena correctamente.
        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View("VwResetPasswordConfirmation");
        }

        // POST: /Account/Logout
        // Cierra la cookie de autenticacion y devuelve al inicio publico.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("VwIndex", "Home");
        }

        // GET: /Account/AccessDenied
        // Vista mostrada cuando el usuario autenticado no tiene permisos.
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("VwAccessDenied");
        }

        // Evita open redirect: solo permite volver a rutas locales del mismo sitio.
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("VwIndex", "Home");
        }

        private AuditContext GetAuditContext()
        {
            return new AuditContext(GetCurrentUserId(), GetClientIp());
        }

        private int? GetCurrentUserId()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(id, out var usuarioId) ? usuarioId : null;
        }

        // Registra la IP que solicito o ejecuto una operacion sensible.
        private string? GetClientIp()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString();
        }

        // Genera un token aleatorio URL-safe para enviar por correo.
        private static string CreateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(32);
            return WebEncoders.Base64UrlEncode(bytes);
        }

        // Convierte el token en SHA-256 hexadecimal para almacenarlo sin guardar
        // el secreto original en la base de datos.
        private static string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToHexString(bytes);
        }
    }
}
