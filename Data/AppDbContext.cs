using Microsoft.EntityFrameworkCore;
using Plantilla_Base.Models.Account;
using Plantilla_Base.Models.Administracion;
using System.Reflection.Emit;

namespace Plantilla_Base.Data
{
    // DbContext principal de Entity Framework Core.
    // Centraliza las tablas del sistema y las reglas de mapeo que luego usan
    // los controladores AccountController y UsuariosApiController.
    public class AppDbContext : DbContext
    {
        // Recibe opciones configuradas en Program.cs, incluida la cadena Npgsql.
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tablas de administracion y seguridad.
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Auditoria> Auditoria { get; set; }
        public DbSet<Dominios> Dominios { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<InicioContenido> InicioContenidos { get; set; }
        public DbSet<SistemaVisualConfig> SistemaVisualConfig { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Roles_Permisos> Roles_Permisos { get; set; }
        public DbSet<Roles_User> Roles_User { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<SolicitudRestaurarContrasena> Solicitud_Restaurar_Contrasena { get; set; }

        // Configura indices, tamanos, obligatoriedad y relaciones.
        // Estas reglas se convierten en migraciones y constraints de base de datos.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Usuarios: entidad central para login, CRUD y recuperacion.
            modelBuilder.Entity<Usuarios>(entity =>
            {
                entity.HasIndex(u => u.Usuario).IsUnique();
                entity.HasIndex(u => u.E_Mail).IsUnique();
                entity.HasIndex(u => u.Identificacion)
                    .IsUnique()
                    .HasFilter("\"Identificacion\" IS NOT NULL");
                entity.Property(u => u.Identificacion).HasMaxLength(30);
                entity.Property(u => u.Usuario).HasMaxLength(60).IsRequired();
                entity.Property(u => u.E_Mail).HasMaxLength(160);
                entity.Property(u => u.Nombre).HasMaxLength(120).IsRequired();
                entity.Property(u => u.Telefono).HasMaxLength(30);
                entity.Property(u => u.Password).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Debe_Cambiar_Password).HasDefaultValue((short)0);
                entity.Property(u => u.Motivo_Actualiza).HasMaxLength(4000);
            });

            // Tokens de recuperacion: se busca por hash y expiran por fecha.
            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasIndex(t => t.TokenHash).IsUnique();
                entity.HasIndex(t => new { t.Id_Usuario, t.Fecha_Expiracion });
                entity.Property(t => t.TokenHash).HasMaxLength(128).IsRequired();
                entity.Property(t => t.Ip_Solicitud).HasMaxLength(80);
                entity.HasOne<Usuarios>()
                    .WithMany()
                    .HasForeignKey(t => t.Id_Usuario)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Solicitudes manuales de restauracion: creadas por el titular y
            // atendidas por un administrador desde el CRUD de usuarios.
            modelBuilder.Entity<SolicitudRestaurarContrasena>(entity =>
            {
                entity.ToTable("Solicitud_Restaurar_Contrasena");
                entity.HasIndex(s => new { s.Id_Usuario, s.Estado, s.Vigente });
                entity.Property(s => s.Identificacion_Solicitud).HasMaxLength(30).IsRequired();
                entity.Property(s => s.Email_Solicitud).HasMaxLength(160).IsRequired();
                entity.Property(s => s.Motivo_Solicitud).HasMaxLength(4000).IsRequired();
                entity.Property(s => s.Estado).HasMaxLength(30).IsRequired();
                entity.Property(s => s.Ip_Solicitud).HasMaxLength(80);
                entity.Property(s => s.UserAgent_Solicitud).HasMaxLength(500);
                entity.Property(s => s.Motivo_Atencion).HasMaxLength(4000);
                entity.Property(s => s.Ip_Atencion).HasMaxLength(80);
                entity.HasOne(s => s.Usuario)
                    .WithMany()
                    .HasForeignKey(s => s.Id_Usuario)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(s => s.UsuarioAtiende)
                    .WithMany()
                    .HasForeignKey(s => s.Id_Usuario_Atiende)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Roles del sistema. Rol debe ser unico para evitar duplicados.
            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasIndex(r => r.Rol).IsUnique();
                entity.Property(r => r.Rol).HasMaxLength(80).IsRequired();
            });

            // Permisos por modulo/accion. El indice evita repetir la misma accion.
            modelBuilder.Entity<Permisos>(entity =>
            {
                entity.HasIndex(p => new { p.TipoPermiso, p.Id_Menu, p.Accion })
                    .IsUnique()
                    .HasFilter("\"Id_Menu\" IS NOT NULL AND \"TipoPermiso\" = 'Menu'");
                entity.HasIndex(p => p.CodigoPermiso)
                    .IsUnique()
                    .HasFilter("\"CodigoPermiso\" IS NOT NULL");
                entity.Property(p => p.TipoPermiso).HasMaxLength(20).HasDefaultValue("Menu").IsRequired();
                entity.Property(p => p.Modulo).HasMaxLength(80).IsRequired();
                entity.Property(p => p.Accion).HasMaxLength(80).IsRequired();
                entity.Property(p => p.Descripcion).HasMaxLength(200);
                entity.Property(p => p.Controlador).HasMaxLength(120);
                entity.Property(p => p.Metodo).HasMaxLength(120);
                entity.Property(p => p.HttpMetodo).HasMaxLength(20);
                entity.Property(p => p.CodigoPermiso).HasMaxLength(300);
                entity.HasOne(p => p.Menu)
                    .WithMany()
                    .HasForeignKey(p => p.Id_Menu)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Dominios o catalogos jerarquicos. Id_Padre apunta a otro dominio.
            modelBuilder.Entity<Dominios>(entity =>
            {
                entity.Property(d => d.Descripcion).HasMaxLength(120).IsRequired();
                entity.Property(d => d.Observacion).HasMaxLength(300);
                entity.Property(d => d.DominioPadre).HasMaxLength(2).HasDefaultValue("No").IsRequired();
                entity.HasOne(d => d.Padre)
                    .WithMany()
                    .HasForeignKey(d => d.Id_Padre)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Menus: estructura jerarquica del menu principal de la aplicacion.
            modelBuilder.Entity<Menus>(entity =>
            {
                entity.Property(m => m.Descripcion).HasMaxLength(255).IsRequired();
                entity.Property(m => m.Tipo).HasMaxLength(255);
                entity.Property(m => m.Controlador).HasMaxLength(255);
                entity.Property(m => m.Vista).HasMaxLength(255);
                entity.Property(m => m.Icono).HasMaxLength(255);
                entity.HasIndex(m => new { m.Controlador, m.Vista });
                entity.HasOne(m => m.Padre)
                    .WithMany()
                    .HasForeignKey(m => m.Id_Padre)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // InicioContenidos: contenido administrable para la pagina publica.
            modelBuilder.Entity<InicioContenido>(entity =>
            {
                entity.HasIndex(i => new { i.TipoContenido, i.Orden });
                entity.Property(i => i.TipoContenido).HasMaxLength(40).IsRequired();
                entity.Property(i => i.Titulo).HasMaxLength(160).IsRequired();
                entity.Property(i => i.Resumen).HasMaxLength(500);
                entity.Property(i => i.ImagenUrl).HasMaxLength(500);
                entity.Property(i => i.EnlaceUrl).HasMaxLength(500);
                entity.Property(i => i.TextoBoton).HasMaxLength(80);
            });

            // SistemaVisualConfig: imagenes globales usadas por layouts y loader.
            modelBuilder.Entity<SistemaVisualConfig>(entity =>
            {
                entity.Property(s => s.LogoUrl).HasMaxLength(500).IsRequired();
                entity.Property(s => s.FaviconUrl).HasMaxLength(500).IsRequired();
                entity.Property(s => s.LoginBackgroundUrl).HasMaxLength(500).IsRequired();
            });

            // Tabla puente entre roles y permisos.
            modelBuilder.Entity<Roles_Permisos>(entity =>
            {
                entity.HasIndex(rp => new { rp.Id_Rol, rp.Id_Permiso }).IsUnique();
                entity.HasOne(rp => rp.Rol)
                    .WithMany()
                    .HasForeignKey(rp => rp.Id_Rol)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(rp => rp.Permiso)
                    .WithMany()
                    .HasForeignKey(rp => rp.Id_Permiso)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Tabla puente entre usuarios y roles.
            modelBuilder.Entity<Roles_User>(entity =>
            {
                entity.HasIndex(ru => new { ru.Id_Usuario, ru.Id_Rol }).IsUnique();
                entity.HasOne(ru => ru.Usuario)
                    .WithMany()
                    .HasForeignKey(ru => ru.Id_Usuario)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(ru => ru.Rol)
                    .WithMany()
                    .HasForeignKey(ru => ru.Id_Rol)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Auditoria: guarda usuario creador/modificador cuando exista.
            modelBuilder.Entity<Auditoria>(entity =>
            {
                entity.Property(a => a.Descripcion).HasMaxLength(4000);
                entity.Property(a => a.Formulario).HasMaxLength(120);
                entity.Property(a => a.Metodo_Ejecutado).HasMaxLength(120);
                entity.HasOne(a => a.UsuarioCrea)
                    .WithMany()
                    .HasForeignKey(a => a.Id_Usuario_Creacion)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(a => a.UsuarioModifica)
                    .WithMany()
                    .HasForeignKey(a => a.Id_Usuario_Modifica)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
