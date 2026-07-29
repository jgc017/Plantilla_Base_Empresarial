using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plantilla_Base.Business.Common;
using Plantilla_Base.Business.Interfaces.Usuarios;
using Plantilla_Base.Data;
using Plantilla_Base.Models.Administracion;
using Plantilla_Base.Models.Dto.Administracion.Usuarios;
using Plantilla_Base.Services.Email;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Plantilla_Base.Business.Services.Usuarios
{
    // Servicio de negocio del CRUD de usuarios.
    // Centraliza duplicados, hashing de contrasena, auditoria y baja logica.
    public class UsuariosService : IUsuarios
    {
        private const string EstadoSolicitudPendiente = "Pendiente";
        private const string EstadoSolicitudUsada = "Usada";

        private readonly AppDbContext _context;
        private readonly ILogger<UsuariosService> _logger;
        private readonly IEmailSender _emailSender;

        public UsuariosService(AppDbContext context, ILogger<UsuariosService> logger, IEmailSender emailSender)
        {
            _context = context;
            _logger = logger;
            _emailSender = emailSender;
        }

        // ExistenUsuarios: permite al controlador decidir si el registro inicial puede ser anonimo.
        public async Task<bool> ExistenUsuarios()
        {
            return await _context.Usuarios.AsNoTracking().AnyAsync();
        }

        // P_InsUsuario: registra un usuario y guarda la contrasena como hash BCrypt.
        public async Task<ServiceResult> P_InsUsuario(DtoUsuarioCreateRequest model, AuditContext audit, bool esRegistroInicial, string? loginUrl)
        {
            var identificacion = model.Identificacion.Trim();
            var nombre = model.Nombre.Trim();
            var telefono = model.Telefono?.Trim();
            var nombreUsuario = model.Usuario.Trim();
            var email = model.E_Mail.Trim().ToLowerInvariant();
            var usuarioNormalizado = nombreUsuario.ToLowerInvariant();
            var passwordFueGenerada = string.IsNullOrWhiteSpace(model.Password);
            var passwordPlano = passwordFueGenerada
                ? GenerarPasswordTemporal()
                : model.Password!.Trim();

            if (esRegistroInicial && string.IsNullOrWhiteSpace(model.Password))
            {
                return ServiceResult.Fail(StatusCodes.Status400BadRequest, "La contrasena es obligatoria para crear el primer usuario.");
            }

            var existe = await _context.Usuarios
                .AnyAsync(u => u.Identificacion == identificacion ||
                               u.Usuario.ToLower() == usuarioNormalizado ||
                               u.E_Mail == email);

            if (existe)
            {
                return ServiceResult.Fail(StatusCodes.Status409Conflict, "La identificacion, usuario o email ya existe");
            }

            var nuevo = new Models.Administracion.Usuarios
            {
                Identificacion = identificacion,
                Nombre = nombre,
                Telefono = telefono,
                Usuario = nombreUsuario,
                E_Mail = email,
                Password = BCrypt.Net.BCrypt.HashPassword(passwordPlano, workFactor: 12),
                Vigente = 1,
                Debe_Cambiar_Password = esRegistroInicial ? (short)0 : (short)1,
                Id_Usuario_Creacion = audit.UserId,
                Fecha_Creacion = DateTime.UtcNow,
                Maquina_Creacion = audit.Machine
            };

            _context.Usuarios.Add(nuevo);

            try
            {
                await _context.SaveChangesAsync();

                if (esRegistroInicial)
                {
                    await AsignarRolSuperUsuarioInicial(nuevo, audit);
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Conflicto registrando usuario {Usuario}", nombreUsuario);
                return ServiceResult.Fail(StatusCodes.Status409Conflict, "La identificacion, usuario o email ya existe");
            }

            var mensaje = esRegistroInicial
                ? "Primer usuario registrado correctamente"
                : "Usuario registrado correctamente. Se proceso el envio de credenciales al correo configurado.";

            if (!esRegistroInicial)
            {
                try
                {
                    await _emailSender.SendNewUserCredentialsAsync(
                        email,
                        nombreUsuario,
                        passwordPlano,
                        loginUrl ?? "/Account/Login");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "No fue posible enviar credenciales al usuario {UsuarioId}", nuevo.Id_Usuario);
                    mensaje = "Usuario registrado correctamente, pero no fue posible enviar el correo de credenciales.";
                }
            }

            return ServiceResult.Success(
                mensaje,
                auditDescription: $"Registro del usuario {nuevo.Usuario} con id {nuevo.Id_Usuario}");
        }

        // F_GetUsuariosList: consulta usuarios para alimentar la grilla principal.
        public async Task<ServiceResult> F_GetUsuariosList()
        {
            var usuarios = await _context.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.Nombre)
                .Select(u => new
                {
                    u.Id_Usuario,
                    u.Identificacion,
                    u.Nombre,
                    u.Telefono,
                    u.Usuario,
                    u.E_Mail,
                    u.Vigente,
                    u.Debe_Cambiar_Password,
                    u.Fecha_Creacion
                })
                .ToListAsync();

            return ServiceResult.Success(data: usuarios);
        }

        // F_GetUsuario: consulta un usuario por id para cargar el modal de actualizacion.
        public async Task<ServiceResult> F_GetUsuario(int idUsuario)
        {
            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id_Usuario == idUsuario);

            if (usuario == null)
            {
                return ServiceResult.Fail(StatusCodes.Status404NotFound, "Usuario no encontrado");
            }

            return ServiceResult.Success(data: new
            {
                usuario.Id_Usuario,
                usuario.Identificacion,
                usuario.Nombre,
                usuario.Telefono,
                usuario.Usuario,
                usuario.E_Mail,
                usuario.Vigente,
                usuario.Debe_Cambiar_Password,
                usuario.Motivo_Actualiza
            }, auditDescription: $"Consulta del usuario {usuario.Usuario} con id {usuario.Id_Usuario}");
        }

        // F_GetUsuarioIdentificacion: consulta un usuario por identificacion para
        // ayudar al administrador a atender una solicitud manual de restauracion.
        public async Task<ServiceResult> F_GetUsuarioIdentificacion(string identificacion)
        {
            var identificacionNormalizada = identificacion?.Trim();
            if (string.IsNullOrWhiteSpace(identificacionNormalizada))
            {
                return ServiceResult.Fail(StatusCodes.Status400BadRequest, "La identificacion es obligatoria.");
            }

            var usuario = await _context.Usuarios
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Identificacion == identificacionNormalizada);

            if (usuario == null)
            {
                return ServiceResult.Fail(StatusCodes.Status404NotFound, "Usuario no encontrado.");
            }

            var now = DateTime.UtcNow;
            var solicitudPendiente = await _context.Solicitud_Restaurar_Contrasena
                .AsNoTracking()
                .Where(s => s.Id_Usuario == usuario.Id_Usuario &&
                            s.Estado == EstadoSolicitudPendiente &&
                            s.Vigente == 1 &&
                            s.Fecha_Expiracion > now)
                .OrderByDescending(s => s.Fecha_Solicitud)
                .Select(s => new
                {
                    s.Id_Solicitud_Restaura,
                    s.Fecha_Solicitud,
                    s.Fecha_Expiracion,
                    s.Motivo_Solicitud
                })
                .FirstOrDefaultAsync();

            return ServiceResult.Success(data: new
            {
                usuario.Id_Usuario,
                usuario.Identificacion,
                usuario.Nombre,
                usuario.Telefono,
                usuario.Usuario,
                usuario.E_Mail,
                usuario.Vigente,
                usuario.Debe_Cambiar_Password,
                TieneSolicitudRestauracionPendiente = solicitudPendiente != null,
                SolicitudRestauracion = solicitudPendiente
            }, auditDescription: $"Consulta del usuario {usuario.Usuario} por identificacion con id {usuario.Id_Usuario}");
        }

        // P_UdpUsuario: actualiza datos generales y estado vigente de un usuario.
        public async Task<ServiceResult> P_UdpUsuario(int idUsuario, DtoUsuarioUpdateRequest model, AuditContext audit)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);
            if (usuario == null)
            {
                return ServiceResult.Fail(StatusCodes.Status404NotFound, "Usuario no existe");
            }

            if (audit.UserId == idUsuario && model.Vigente == 0)
            {
                return ServiceResult.Fail(StatusCodes.Status400BadRequest, "No puedes desactivar tu propio usuario.");
            }

            var nombreUsuario = model.Usuario.Trim();
            var email = model.E_Mail.Trim().ToLowerInvariant();
            var usuarioNormalizado = nombreUsuario.ToLowerInvariant();
            var identificacion = model.Identificacion.Trim();
            var motivo = model.Motivo_Actualiza.Trim();

            var duplicado = await _context.Usuarios
                .AnyAsync(x => x.Id_Usuario != idUsuario &&
                    (x.Identificacion == identificacion ||
                     x.Usuario.ToLower() == usuarioNormalizado ||
                     x.E_Mail == email));

            if (duplicado)
            {
                return ServiceResult.Fail(StatusCodes.Status409Conflict, "La identificacion, usuario o email ya existe");
            }

            usuario.Identificacion = identificacion;
            usuario.Nombre = model.Nombre.Trim();
            usuario.Telefono = model.Telefono?.Trim();
            usuario.Usuario = nombreUsuario;
            usuario.E_Mail = email;
            usuario.Vigente = model.Vigente;
            usuario.Motivo_Actualiza = motivo;
            usuario.Id_Usuario_Modifica = audit.UserId;
            usuario.Fecha_Modifica = DateTime.UtcNow;
            usuario.Maquina_Modifica = audit.Machine;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogWarning(ex, "Conflicto actualizando usuario {IdUsuario}", idUsuario);
                return ServiceResult.Fail(StatusCodes.Status409Conflict, "La identificacion, usuario o email ya existe");
            }

            return ServiceResult.Success(
                "Usuario actualizado correctamente",
                auditDescription: $"Actualizacion del usuario {usuario.Usuario} con id {usuario.Id_Usuario}. Motivo: {motivo}");
        }

        // P_RestaurarContrasenaSolicitud: cambia la contrasena temporal solo si
        // existe una solicitud vigente creada por el usuario titular.
        public async Task<ServiceResult> P_RestaurarContrasenaSolicitud(
            int idUsuario,
            DtoUsuarioRestorePasswordRequest model,
            AuditContext audit,
            string? loginUrl)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id_Usuario == idUsuario && u.Vigente == 1);
            if (usuario == null)
            {
                return ServiceResult.Fail(StatusCodes.Status404NotFound, "Usuario no encontrado o inactivo.");
            }

            var now = DateTime.UtcNow;
            var solicitud = await _context.Solicitud_Restaurar_Contrasena
                .Where(s => s.Id_Usuario == idUsuario &&
                            s.Estado == EstadoSolicitudPendiente &&
                            s.Vigente == 1 &&
                            s.Fecha_Expiracion > now)
                .OrderByDescending(s => s.Fecha_Solicitud)
                .FirstOrDefaultAsync();

            if (solicitud == null)
            {
                return ServiceResult.Fail(
                    StatusCodes.Status400BadRequest,
                    "No existe una solicitud pendiente de restauracion para este usuario.");
            }

            var passwordFueGenerada = string.IsNullOrWhiteSpace(model.Password);
            var passwordPlano = passwordFueGenerada
                ? GenerarPasswordTemporal()
                : model.Password!.Trim();

            usuario.Password = BCrypt.Net.BCrypt.HashPassword(passwordPlano, workFactor: 12);
            usuario.Debe_Cambiar_Password = 1;
            usuario.Id_Usuario_Modifica = audit.UserId;
            usuario.Fecha_Modifica = now;
            usuario.Maquina_Modifica = audit.Machine;

            solicitud.Estado = EstadoSolicitudUsada;
            solicitud.Vigente = 0;
            solicitud.Fecha_Atencion = now;
            solicitud.Id_Usuario_Atiende = audit.UserId;
            solicitud.Motivo_Atencion = "Restauracion de contrasena atendida por administrador.";
            solicitud.Ip_Atencion = audit.Machine;

            await using var transaction = await _context.Database.BeginTransactionAsync();
            await _context.SaveChangesAsync();

            var mensaje = "Contrasena temporal restaurada correctamente. El usuario debera cambiarla al iniciar sesion.";

            try
            {
                if (!string.IsNullOrWhiteSpace(usuario.E_Mail))
                {
                    await _emailSender.SendNewUserCredentialsAsync(
                        usuario.E_Mail,
                        usuario.Usuario,
                        passwordPlano,
                        loginUrl ?? "/Account/Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No fue posible enviar la contrasena restaurada al usuario {UsuarioId}", usuario.Id_Usuario);
                if (passwordFueGenerada)
                {
                    await transaction.RollbackAsync();

                    return ServiceResult.Fail(
                        StatusCodes.Status502BadGateway,
                        "No fue posible enviar el correo. La solicitud sigue pendiente; intenta nuevamente o escribe una contrasena temporal manual.");
                }

                mensaje = "Contrasena temporal restaurada, pero no fue posible enviar el correo.";
            }

            await transaction.CommitAsync();

            return ServiceResult.Success(
                mensaje,
                auditDescription: $"Restauracion de contrasena del usuario {usuario.Usuario} con id {usuario.Id_Usuario}, solicitud {solicitud.Id_Solicitud_Restaura}");
        }

        // P_DeleteUsuario: realiza baja logica y evita que el usuario actual se elimine a si mismo.
        public async Task<ServiceResult> P_DeleteUsuario(int idUsuario, AuditContext audit)
        {
            try
            {
                if (audit.UserId == idUsuario)
                {
                    return ServiceResult.Fail(StatusCodes.Status400BadRequest, "No puedes eliminar tu propio usuario.");
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id_Usuario == idUsuario);
                if (usuario == null)
                {
                    return ServiceResult.Fail(StatusCodes.Status404NotFound, "El usuario no existe.");
                }

                if (usuario.Vigente == 0)
                {
                    return ServiceResult.Success(
                        "El usuario ya se encontraba inactivo.",
                        auditDescription: $"Eliminacion logica del usuario {usuario.Usuario} con id {usuario.Id_Usuario}");
                }

                usuario.Vigente = 0;
                usuario.Id_Usuario_Modifica = audit.UserId;
                usuario.Fecha_Modifica = DateTime.UtcNow;
                usuario.Maquina_Modifica = audit.Machine;

                await _context.SaveChangesAsync();

                return ServiceResult.Success(
                    "El usuario fue marcado como inactivo correctamente.",
                    auditDescription: $"Eliminacion logica del usuario {usuario.Usuario} con id {usuario.Id_Usuario}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando usuario {IdUsuario}", idUsuario);
                return ServiceResult.Fail(StatusCodes.Status500InternalServerError, "Ocurrio un error al eliminar el usuario.");
            }
        }

        // Al crear el primer usuario se intenta asignar el rol 1, reservado como super usuario.
        private async Task AsignarRolSuperUsuarioInicial(Models.Administracion.Usuarios usuario, AuditContext audit)
        {
            var rolSuperUsuarioExiste = await _context.Roles
                .AsNoTracking()
                .AnyAsync(r => r.Id_Rol == 1 && r.Vigente == 1);

            if (!rolSuperUsuarioExiste)
            {
                _logger.LogWarning("Primer usuario {UsuarioId} creado sin rol super usuario porque no existe un rol activo con Id_Rol 1.", usuario.Id_Usuario);
                return;
            }

            _context.Roles_User.Add(new Roles_User
            {
                Id_Usuario = usuario.Id_Usuario,
                Id_Rol = 1,
                Vigente = 1,
                Id_Usuario_Creacion = audit.UserId ?? usuario.Id_Usuario,
                Fecha_Creacion = DateTime.UtcNow,
                Maquina_Creacion = audit.Machine
            });

            await _context.SaveChangesAsync();
        }

        // Genera una contrasena temporal compatible con la politica actual:
        // minimo 10 caracteres, mayuscula, minuscula y numero.
        private static string GenerarPasswordTemporal()
        {
            const string minusculas = "abcdefghijkmnopqrstuvwxyz";
            const string mayusculas = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string numeros = "23456789";
            const string simbolos = "!@$%*?";
            const string todos = minusculas + mayusculas + numeros + simbolos;

            var caracteres = new[]
            {
                minusculas[RandomNumberGenerator.GetInt32(minusculas.Length)],
                mayusculas[RandomNumberGenerator.GetInt32(mayusculas.Length)],
                numeros[RandomNumberGenerator.GetInt32(numeros.Length)],
                simbolos[RandomNumberGenerator.GetInt32(simbolos.Length)]
            }.ToList();

            while (caracteres.Count < 14)
            {
                caracteres.Add(todos[RandomNumberGenerator.GetInt32(todos.Length)]);
            }

            return new string(caracteres
                .OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue))
                .ToArray());
        }
    }
}
