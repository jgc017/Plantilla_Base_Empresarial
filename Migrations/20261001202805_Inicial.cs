using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dominios",
                columns: table => new
                {
                    Id_Dominio = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Id_Padre = table.Column<int>(type: "integer", nullable: true),
                    Observacion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DominioPadre = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false, defaultValue: "No"),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Crea = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dominios", x => x.Id_Dominio);
                    table.ForeignKey(
                        name: "FK_Dominios_Dominios_Id_Padre",
                        column: x => x.Id_Padre,
                        principalTable: "Dominios",
                        principalColumn: "Id_Dominio",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InicioContenidos",
                columns: table => new
                {
                    Id_InicioContenido = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoContenido = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Resumen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Contenido = table.Column<string>(type: "text", nullable: true),
                    ImagenUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EnlaceUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TextoBoton = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    MostrarEnInicio = table.Column<short>(type: "smallint", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InicioContenidos", x => x.Id_InicioContenido);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id_Menu = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Id_Padre = table.Column<int>(type: "integer", nullable: true),
                    Posicion = table.Column<int>(type: "integer", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Controlador = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Vista = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Icono = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id_Menu);
                    table.ForeignKey(
                        name: "FK_Menus_Menus_Id_Padre",
                        column: x => x.Id_Padre,
                        principalTable: "Menus",
                        principalColumn: "Id_Menu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id_Rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rol = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id_Rol);
                });

            migrationBuilder.CreateTable(
                name: "SistemaVisualConfig",
                columns: table => new
                {
                    Id_SistemaVisualConfig = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FaviconUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LoginBackgroundUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SistemaVisualConfig", x => x.Id_SistemaVisualConfig);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Identificacion = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Telefono = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Usuario = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    E_Mail = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: true),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Debe_Cambiar_Password = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true),
                    Motivo_Actualiza = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id_Usuario);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id_Permiso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Menu = table.Column<int>(type: "integer", nullable: true),
                    TipoPermiso = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Menu"),
                    Modulo = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Accion = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Controlador = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Metodo = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    HttpMetodo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CodigoPermiso = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.Id_Permiso);
                    table.ForeignKey(
                        name: "FK_Permisos_Menus_Id_Menu",
                        column: x => x.Id_Menu,
                        principalTable: "Menus",
                        principalColumn: "Id_Menu",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id_Auditoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Formulario = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Metodo_Ejecutado = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id_Auditoria);
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuarios_Id_Usuario_Creacion",
                        column: x => x.Id_Usuario_Creacion,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuarios_Id_Usuario_Modifica",
                        column: x => x.Id_Usuario_Modifica,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id_PasswordResetToken = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Usuario = table.Column<int>(type: "integer", nullable: false),
                    TokenHash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Fecha_Expiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Fecha_Uso = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Ip_Solicitud = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id_PasswordResetToken);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Usuarios_Id_Usuario",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles_User",
                columns: table => new
                {
                    Id_Roles_User = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Usuario = table.Column<int>(type: "integer", nullable: false),
                    Id_Rol = table.Column<int>(type: "integer", nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles_User", x => x.Id_Roles_User);
                    table.ForeignKey(
                        name: "FK_Roles_User_Roles_Id_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "Roles",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_User_Usuarios_Id_Usuario",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Solicitud_Restaurar_Contrasena",
                columns: table => new
                {
                    Id_Solicitud_Restaura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Usuario = table.Column<int>(type: "integer", nullable: false),
                    Identificacion_Solicitud = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Email_Solicitud = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Motivo_Solicitud = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Fecha_Solicitud = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Fecha_Expiracion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ip_Solicitud = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    UserAgent_Solicitud = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Fecha_Atencion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Id_Usuario_Atiende = table.Column<int>(type: "integer", nullable: true),
                    Motivo_Atencion = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    Ip_Atencion = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitud_Restaurar_Contrasena", x => x.Id_Solicitud_Restaura);
                    table.ForeignKey(
                        name: "FK_Solicitud_Restaurar_Contrasena_Usuarios_Id_Usuario",
                        column: x => x.Id_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Solicitud_Restaurar_Contrasena_Usuarios_Id_Usuario_Atiende",
                        column: x => x.Id_Usuario_Atiende,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles_Permisos",
                columns: table => new
                {
                    Id_Rol_Permiso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id_Rol = table.Column<int>(type: "integer", nullable: false),
                    Id_Permiso = table.Column<int>(type: "integer", nullable: false),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles_Permisos", x => x.Id_Rol_Permiso);
                    table.ForeignKey(
                        name: "FK_Roles_Permisos_Permisos_Id_Permiso",
                        column: x => x.Id_Permiso,
                        principalTable: "Permisos",
                        principalColumn: "Id_Permiso",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Permisos_Roles_Id_Rol",
                        column: x => x.Id_Rol,
                        principalTable: "Roles",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_Id_Usuario_Creacion",
                table: "Auditoria",
                column: "Id_Usuario_Creacion");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_Id_Usuario_Modifica",
                table: "Auditoria",
                column: "Id_Usuario_Modifica");

            migrationBuilder.CreateIndex(
                name: "IX_Dominios_Id_Padre",
                table: "Dominios",
                column: "Id_Padre");

            migrationBuilder.CreateIndex(
                name: "IX_InicioContenidos_TipoContenido_Orden",
                table: "InicioContenidos",
                columns: new[] { "TipoContenido", "Orden" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Controlador_Vista",
                table: "Menus",
                columns: new[] { "Controlador", "Vista" });

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Id_Padre",
                table: "Menus",
                column: "Id_Padre");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_Id_Usuario_Fecha_Expiracion",
                table: "PasswordResetTokens",
                columns: new[] { "Id_Usuario", "Fecha_Expiracion" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_TokenHash",
                table: "PasswordResetTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_CodigoPermiso",
                table: "Permisos",
                column: "CodigoPermiso",
                unique: true,
                filter: "\"CodigoPermiso\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Id_Menu",
                table: "Permisos",
                column: "Id_Menu");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_TipoPermiso_Id_Menu_Accion",
                table: "Permisos",
                columns: new[] { "TipoPermiso", "Id_Menu", "Accion" },
                unique: true,
                filter: "\"Id_Menu\" IS NOT NULL AND \"TipoPermiso\" = 'Menu'");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Rol",
                table: "Roles",
                column: "Rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_Id_Permiso",
                table: "Roles_Permisos",
                column: "Id_Permiso");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_Id_Rol_Id_Permiso",
                table: "Roles_Permisos",
                columns: new[] { "Id_Rol", "Id_Permiso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_Id_Rol",
                table: "Roles_User",
                column: "Id_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_Id_Usuario_Id_Rol",
                table: "Roles_User",
                columns: new[] { "Id_Usuario", "Id_Rol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Restaurar_Contrasena_Id_Usuario_Atiende",
                table: "Solicitud_Restaurar_Contrasena",
                column: "Id_Usuario_Atiende");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Restaurar_Contrasena_Id_Usuario_Estado_Vigente",
                table: "Solicitud_Restaurar_Contrasena",
                columns: new[] { "Id_Usuario", "Estado", "Vigente" });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_E_Mail",
                table: "Usuarios",
                column: "E_Mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Identificacion",
                table: "Usuarios",
                column: "Identificacion",
                unique: true,
                filter: "\"Identificacion\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Usuario",
                table: "Usuarios",
                column: "Usuario",
                unique: true);

            SembrarDatosIniciales(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Dominios");

            migrationBuilder.DropTable(
                name: "InicioContenidos");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "Roles_Permisos");

            migrationBuilder.DropTable(
                name: "Roles_User");

            migrationBuilder.DropTable(
                name: "SistemaVisualConfig");

            migrationBuilder.DropTable(
                name: "Solicitud_Restaurar_Contrasena");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Menus");
        }

        private static void SembrarDatosIniciales(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO "Roles" ("Id_Rol", "Rol", "Vigente", "Fecha_Creacion", "Maquina_Creacion") VALUES
                (1, 'Super Usuario', 1, now(), 'MigracionInicial'),
                (2, 'Administrador', 1, now(), 'MigracionInicial') ON CONFLICT ("Id_Rol") DO NOTHING;

                INSERT INTO "Dominios" ("Id_Dominio", "Descripcion", "Id_Padre", "Vigente", "Fecha_Creacion", "Maquina_Creacion", "DominioPadre") VALUES
                (1, 'SIN DATOS', NULL, 1, now(), 'MigracionInicial', 'Si'),
                (2, 'Dominio', 1, 1, now(), 'MigracionInicial', 'Si'),
                (3, 'Permisos', 2, 1, now(), 'MigracionInicial', 'Si'),
                (4, 'Ver', 3, 1, now(), 'MigracionInicial', 'No'),
                (5, 'Crear', 3, 1, now(), 'MigracionInicial', 'No'),
                (6, 'Consultar', 3, 1, now(), 'MigracionInicial', 'No'),
                (7, 'Actualizar', 3, 1, now(), 'MigracionInicial', 'No'),
                (8, 'Eliminar', 3, 1, now(), 'MigracionInicial', 'No'),
                (9, 'Asignar', 3, 1, now(), 'MigracionInicial', 'No'),
                (10, 'TipoContenidoInicio', 2, 1, now(), 'MigracionInicial', 'Si'),
                (11, 'Slider', 10, 1, now(), 'MigracionInicial', 'No'),
                (12, 'Contacto', 10, 1, now(), 'MigracionInicial', 'No') ON CONFLICT ("Id_Dominio") DO NOTHING;

                INSERT INTO "Menus" ("Id_Menu", "Descripcion", "Id_Padre", "Posicion", "Tipo", "Controlador", "Vista", "Icono", "Vigente", "Fecha_Creacion", "Maquina_Creacion") VALUES
                (1, 'Administracion', NULL, 2, 'Modulo', NULL, NULL, 'fa-solid fa-gear', 1, now(), 'MigracionInicial'),
                (2, 'Usuarios', 1, 1, 'Formulario', 'Usuarios', 'VwUsuarios', 'fa-solid fa-users', 1, now(), 'MigracionInicial'),
                (3, 'Roles', 1, 2, 'Formulario', 'Roles', 'VwRoles', 'fa-solid fa-user-shield', 1, now(), 'MigracionInicial'),
                (4, 'Permisos', 1, 3, 'Formulario', 'Permisos', 'VwPermisos', 'fa-solid fa-key', 1, now(), 'MigracionInicial'),
                (5, 'Dominios', 1, 4, 'Formulario', 'Dominios', 'VwDominios', 'fa-solid fa-sitemap', 1, now(), 'MigracionInicial'),
                (6, 'Inicio Contenidos', 1, 5, 'Formulario', 'InicioAdmin', 'VwInicioAdmin', 'fa-solid fa-house', 1, now(), 'MigracionInicial'),
                (7, 'Sistema Config', 1, 6, 'Formulario', 'SistemaConfig', 'VwSistemaConfig', 'fa-solid fa-image', 1, now(), 'MigracionInicial') ON CONFLICT ("Id_Menu") DO NOTHING;

                INSERT INTO "Permisos" ("Id_Permiso", "TipoPermiso", "Id_Menu", "Modulo", "Accion", "Descripcion", "Controlador", "Metodo", "HttpMetodo", "CodigoPermiso", "Vigente", "Fecha_Creacion", "Maquina_Creacion") VALUES
                (1, 'Menu', 2, 'Usuarios', 'Ver', 'Ver Usuarios', 'Usuarios', 'VwUsuarios', 'GET', 'Menu:Usuarios:VwUsuarios:Ver', 1, now(), 'MigracionInicial'),
                (2, 'Menu', 3, 'Roles', 'Ver', 'Ver Roles', 'Roles', 'VwRoles', 'GET', 'Menu:Roles:VwRoles:Ver', 1, now(), 'MigracionInicial'),
                (3, 'Menu', 4, 'Permisos', 'Ver', 'Ver Permisos', 'Permisos', 'VwPermisos', 'GET', 'Menu:Permisos:VwPermisos:Ver', 1, now(), 'MigracionInicial'),
                (4, 'Menu', 5, 'Dominios', 'Ver', 'Ver Dominios', 'Dominios', 'VwDominios', 'GET', 'Menu:Dominios:VwDominios:Ver', 1, now(), 'MigracionInicial'),
                (5, 'Menu', 6, 'InicioAdmin', 'Ver', 'Ver Inicio Contenidos', 'InicioAdmin', 'VwInicioAdmin', 'GET', 'Menu:InicioAdmin:VwInicioAdmin:Ver', 1, now(), 'MigracionInicial'),
                (6, 'Menu', 7, 'SistemaConfig', 'Ver', 'Ver Sistema Config', 'SistemaConfig', 'VwSistemaConfig', 'GET', 'Menu:SistemaConfig:VwSistemaConfig:Ver', 1, now(), 'MigracionInicial'),
                
                (7, 'Metodo', 2, 'Usuarios', 'Consultar', 'Permite consultar el detalle de registros.', 'UsuariosApi', 'F_GetUsuario', 'GET', 'USUARIOSAPI.F_GETUSUARIO.GET', 1, now(), 'MigracionInicial'),
                (8, 'Metodo', 2, 'Usuarios', 'Eliminar', 'Permite eliminar o inactivar registros.', 'UsuariosApi', 'P_DeleteUsuario', 'DELETE', 'USUARIOSAPI.P_DELETEUSUARIO.DELETE', 1, now(), 'MigracionInicial'),
                (9, 'Metodo', 2, 'Usuarios', 'Actualizar', 'Permite modificar informacion existente.', 'UsuariosApi', 'P_UdpUsuario', 'PUT', 'USUARIOSAPI.P_UDPUSUARIO.PUT', 1, now(), 'MigracionInicial'),
                (10, 'Metodo', 2, 'Usuarios', 'Asignar', 'Permite asignar roles al usuario.', 'RolesUserApi', 'Asignar', 'PUT', 'ROLESUSERAPI.ASIGNAR.PUT', 1, now(), 'MigracionInicial'),
                (11, 'Metodo', 2, 'Usuarios', 'Consultar', 'Permite consultar roles del usuario.', 'RolesUserApi', 'GetIdUserRoles', 'GET', 'ROLESUSERAPI.GETIDUSERROLES.GET', 1, now(), 'MigracionInicial'),

                (12, 'Metodo', 3, 'Roles', 'Consultar', 'Permite consultar el detalle de registros.', 'RolesApi', 'F_GetRol', 'GET', 'ROLESAPI.F_GETROL.GET', 1, now(), 'MigracionInicial'),
                (13, 'Metodo', 3, 'Roles', 'Eliminar', 'Permite eliminar o inactivar registros.', 'RolesApi', 'P_DeleteRol', 'DELETE', 'ROLESAPI.P_DELETEROL.DELETE', 1, now(), 'MigracionInicial'),
                (14, 'Metodo', 3, 'Roles', 'Crear', 'Permite registrar nueva informacion.', 'RolesApi', 'P_InsRol', 'POST', 'ROLESAPI.P_INSROL.POST', 1, now(), 'MigracionInicial'),
                (15, 'Metodo', 3, 'Roles', 'Actualizar', 'Permite actualizar informacion existente.', 'RolesApi', 'P_UdpRol', 'PUT', 'ROLESAPI.P_UDPROL.PUT', 1, now(), 'MigracionInicial'),

                (16, 'Metodo', 4, 'Permisos', 'Consultar', 'Permite consultar el detalle de registros.', 'PermisosApi', 'F_GetPermiso', 'GET', 'PERMISOSAPI.F_GETPERMISO.GET', 1, now(), 'MigracionInicial'),
                (17, 'Metodo', 4, 'Permisos', 'Asignar', 'Permite asignar o actualizar relaciones.', 'PermisosApi', 'F_GetPermisoRol', 'GET', 'PERMISOSAPI.F_GETPERMISOROL.GET', 1, now(), 'MigracionInicial'),
                (18, 'Metodo', 4, 'Permisos', 'Consultar', 'Permite consultar el detalle de registros.', 'PermisosApi', 'F_GetRolesPorPermiso', 'GET', 'PERMISOSAPI.F_GETROLESPORPERMISO.GET', 1, now(), 'MigracionInicial'),
                (19, 'Metodo', 4, 'Permisos', 'Eliminar', 'Permite eliminar o inactivar registros.', 'PermisosApi', 'P_DeletePermiso', 'DELETE', 'PERMISOSAPI.P_DELETEPERMISO.DELETE', 1, now(), 'MigracionInicial'),
                (20, 'Metodo', 4, 'Permisos', 'Asignar', 'Permite asignar o actualizar relaciones.', 'PermisosApi', 'P_DeletePermisoRol', 'DELETE', 'PERMISOSAPI.P_DELETEPERMISOROL.DELETE', 1, now(), 'MigracionInicial'),
                (21, 'Metodo', 4, 'Permisos', 'Crear', 'Permite registrar nueva informacion.', 'PermisosApi', 'P_InsPermiso', 'POST', 'PERMISOSAPI.P_INSPERMISO.POST', 1, now(), 'MigracionInicial'),
                (22, 'Metodo', 4, 'Permisos', 'Asignar', 'Permite asignar o actualizar relaciones.', 'PermisosApi', 'P_InsPermisoRol', 'POST', 'PERMISOSAPI.P_INSPERMISOROL.POST', 1, now(), 'MigracionInicial'),
                (23, 'Metodo', 4, 'Permisos', 'Actualizar', 'Permite modificar informacion existente.', 'PermisosApi', 'P_UdpPermiso', 'PUT', 'PERMISOSAPI.P_UDPPERMISO.PUT', 1, now(), 'MigracionInicial'),
                (24, 'Metodo', 4, 'Permisos', 'Asignar', 'Permite asignar o actualizar relaciones.', 'PermisosApi', 'P_UdpRolesPermiso', 'PUT', 'PERMISOSAPI.P_UDPROLESPERMISO.PUT', 1, now(), 'MigracionInicial'),
                (25, 'Metodo', 4, 'Permisos', 'Consultar', 'Permite consultar el detalle de registros.', 'PermisosMetodosApi', 'F_GetPermisoMetodo', 'GET', 'PERMISOSMETODOSAPI.F_GETPERMISOMETODO.GET', 1, now(), 'MigracionInicial'),
                (26, 'Metodo', 4, 'Permisos', 'Eliminar', 'Permite eliminar o inactivar registros.', 'PermisosMetodosApi', 'P_DeletePermisoMetodo', 'DELETE', 'PERMISOSMETODOSAPI.P_DELETEPERMISOMETODO.DELETE', 1, now(), 'MigracionInicial'),
                (27, 'Metodo', 4, 'Permisos', 'Sincronizar', 'Permite sincronizar informacion automatica.', 'PermisosMetodosApi', 'P_SyncPermisosMetodos', 'POST', 'PERMISOSMETODOSAPI.P_SYNCPERMISOSMETODOS.POST', 1, now(), 'MigracionInicial'),
                (28, 'Metodo', 4, 'Permisos', 'Actualizar', 'Permite modificar informacion existente.', 'PermisosMetodosApi', 'P_UdpPermisoMetodo', 'PUT', 'PERMISOSMETODOSAPI.P_UDPPERMISOMETODO.PUT', 1, now(), 'MigracionInicial'),

                (29, 'Metodo', 5, 'Dominios', 'Consultar', 'Permite consultar el detalle de registros.', 'DominiosApi', 'F_GetDominio', 'GET', 'DOMINIOSAPI.F_GETDOMINIO.GET', 1, now(), 'MigracionInicial'),
                (30, 'Metodo', 5, 'Dominios', 'Eliminar', 'Permite eliminar o inactivar registros.', 'DominiosApi', 'P_DeleteDominio', 'DELETE', 'DOMINIOSAPI.P_DELETEDOMINIO.DELETE', 1, now(), 'MigracionInicial'),
                (31, 'Metodo', 5, 'Dominios', 'Crear', 'Permite registrar nueva informacion.', 'DominiosApi', 'P_InsDominio', 'POST', 'DOMINIOSAPI.P_INSDOMINIO.POST', 1, now(), 'MigracionInicial'),
                (32, 'Metodo', 5, 'Dominios', 'Actualizar', 'Permite modificar informacion existente.', 'DominiosApi', 'P_UdpDominio', 'PUT', 'DOMINIOSAPI.P_UDPDOMINIO.PUT', 1, now(), 'MigracionInicial'),

                (33, 'Metodo', 6, 'InicioAdmin', 'Consultar', 'Permite consultar registros de inicio.', 'InicioAdminApi', 'F_GetInicioContenido', 'GET', 'INICIOADMINAPI.F_GETINICIOCONTENIDO.GET', 1, now(), 'MigracionInicial'),
                (34, 'Metodo', 6, 'InicioAdmin', 'Eliminar', 'Permite eliminar registros de inicio.', 'InicioAdminApi', 'P_DeleteInicioContenido', 'DELETE', 'INICIOADMINAPI.P_DELETEINICIOCONTENIDO.DELETE', 1, now(), 'MigracionInicial'),
                (35, 'Metodo', 6, 'InicioAdmin', 'Crear', 'Permite crear registros de inicio.', 'InicioAdminApi', 'P_InsInicioContenido', 'POST', 'INICIOADMINAPI.P_INSINICIOCONTENIDO.POST', 1, now(), 'MigracionInicial'),
                (36, 'Metodo', 6, 'InicioAdmin', 'Actualizar', 'Permite actualizar registros de inicio.', 'InicioAdminApi', 'P_UdpInicioContenido', 'PUT', 'INICIOADMINAPI.P_UDPINICIOCONTENIDO.PUT', 1, now(), 'MigracionInicial'),
                (37, 'Metodo', 6, 'InicioAdmin', 'Cargar', 'Permite cargar imagenes de inicio.', 'InicioAdminApi', 'P_UploadImagenInicio', 'POST', 'INICIOADMINAPI.P_UPLOADIMAGENINICIO.POST', 1, now(), 'MigracionInicial'),

                (38, 'Metodo', 7, 'SistemaConfig', 'Consultar', 'Permite consultar configuraciones.', 'SistemaConfigApi', 'F_GetSistemaVisualConfig', 'GET', 'SISTEMACONFIGAPI.F_GETSISTEMAVISUALCONFIG.GET', 1, now(), 'MigracionInicial'),
                (39, 'Metodo', 7, 'SistemaConfig', 'Actualizar', 'Permite modificar configuraciones.', 'SistemaConfigApi', 'P_UdpSistemaVisualConfig', 'PUT', 'SISTEMACONFIGAPI.P_UDPSISTEMAVISUALCONFIG.PUT', 1, now(), 'MigracionInicial'),
                (40, 'Metodo', 7, 'SistemaConfig', 'Cargar', 'Permite cargar imagenes globales.', 'SistemaConfigApi', 'P_UploadImagenSistema', 'POST', 'SISTEMACONFIGAPI.P_UPLOADIMAGENSISTEMA.POST', 1, now(), 'MigracionInicial'),
                (41, 'Metodo', 7, 'SistemaConfig', 'Cargar', 'Permite cargar videos globales.', 'SistemaConfigApi', 'P_UploadVideoSistema', 'POST', 'SISTEMACONFIGAPI.P_UPLOADVIDEOSISTEMA.POST', 1, now(), 'MigracionInicial')
                ON CONFLICT ("Id_Permiso") DO NOTHING;

                INSERT INTO "Roles_Permisos" ("Id_Rol", "Id_Permiso", "Vigente", "Fecha_Creacion", "Maquina_Creacion")
                SELECT 1, "Id_Permiso", 1, now(), 'MigracionInicial' FROM "Permisos"
                ON CONFLICT ("Id_Rol", "Id_Permiso") DO NOTHING;

                INSERT INTO "Roles_Permisos" ("Id_Rol", "Id_Permiso", "Vigente", "Fecha_Creacion", "Maquina_Creacion")
                SELECT 2, "Id_Permiso", 1, now(), 'MigracionInicial' 
                FROM "Permisos" 
                WHERE "TipoPermiso" = 'Menu' AND "Id_Permiso" NOT IN (2, 3)
                UNION ALL
                SELECT 2, "Id_Permiso", 1, now(), 'MigracionInicial' 
                FROM "Permisos" 
                WHERE "TipoPermiso" = 'Metodo' AND "Modulo" NOT IN ('Roles', 'Permisos')
                ON CONFLICT ("Id_Rol", "Id_Permiso") DO NOTHING;

                SELECT setval(pg_get_serial_sequence('"Roles"', 'Id_Rol'), COALESCE((SELECT MAX("Id_Rol") FROM "Roles"), 1), true);
                SELECT setval(pg_get_serial_sequence('"Dominios"', 'Id_Dominio'), COALESCE((SELECT MAX("Id_Dominio") FROM "Dominios"), 1), true);
                SELECT setval(pg_get_serial_sequence('"Menus"', 'Id_Menu'), COALESCE((SELECT MAX("Id_Menu") FROM "Menus"), 1), true);
                SELECT setval(pg_get_serial_sequence('"Permisos"', 'Id_Permiso'), COALESCE((SELECT MAX("Id_Permiso") FROM "Permisos"), 1), true);
                SELECT setval(pg_get_serial_sequence('"Roles_Permisos"', 'Id_Rol_Permiso'), COALESCE((SELECT MAX("Id_Rol_Permiso") FROM "Roles_Permisos"), 1), true);
                """);
        }
    }
}
