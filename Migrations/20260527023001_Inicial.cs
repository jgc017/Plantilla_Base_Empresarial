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
                    Descripcion = table.Column<string>(type: "text", nullable: false),
                    Id_Padre = table.Column<int>(type: "integer", nullable: true),
                    Observacion = table.Column<string>(type: "text", nullable: true),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Crea = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true),
                    PadreId_Dominio = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dominios", x => x.Id_Dominio);
                    table.ForeignKey(
                        name: "FK_Dominios_Dominios_PadreId_Dominio",
                        column: x => x.PadreId_Dominio,
                        principalTable: "Dominios",
                        principalColumn: "Id_Dominio");
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    Id_Permiso = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Modulo = table.Column<string>(type: "text", nullable: false),
                    Accion = table.Column<string>(type: "text", nullable: false),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id_Rol = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rol = table.Column<string>(type: "text", nullable: false),
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
                name: "Usuarios",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: true),
                    Usuario = table.Column<string>(type: "text", nullable: false),
                    E_Mail = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Usuarios", x => x.Id_Usuario);
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
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true),
                    RolId_Rol = table.Column<int>(type: "integer", nullable: false),
                    PermisoId_Permiso = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles_Permisos", x => x.Id_Rol_Permiso);
                    table.ForeignKey(
                        name: "FK_Roles_Permisos_Permisos_PermisoId_Permiso",
                        column: x => x.PermisoId_Permiso,
                        principalTable: "Permisos",
                        principalColumn: "Id_Permiso",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Permisos_Roles_RolId_Rol",
                        column: x => x.RolId_Rol,
                        principalTable: "Roles",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auditoria",
                columns: table => new
                {
                    Id_Auditoria = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Descripcion = table.Column<string>(type: "text", nullable: true),
                    Vigente = table.Column<short>(type: "smallint", nullable: false),
                    Id_Usuario_Creacion = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Creacion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Maquina_Creacion = table.Column<string>(type: "text", nullable: true),
                    Id_Usuario_Modifica = table.Column<int>(type: "integer", nullable: true),
                    Fecha_Modifica = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true),
                    UsuarioCreaId_Usuario = table.Column<int>(type: "integer", nullable: true),
                    UsuarioModificaId_Usuario = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auditoria", x => x.Id_Auditoria);
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuarios_UsuarioCreaId_Usuario",
                        column: x => x.UsuarioCreaId_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario");
                    table.ForeignKey(
                        name: "FK_Auditoria_Usuarios_UsuarioModificaId_Usuario",
                        column: x => x.UsuarioModificaId_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario");
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
                    Maquina_Modifica = table.Column<string>(type: "text", nullable: true),
                    UsuarioId_Usuario = table.Column<int>(type: "integer", nullable: false),
                    RolId_Rol = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles_User", x => x.Id_Roles_User);
                    table.ForeignKey(
                        name: "FK_Roles_User_Roles_RolId_Rol",
                        column: x => x.RolId_Rol,
                        principalTable: "Roles",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_User_Usuarios_UsuarioId_Usuario",
                        column: x => x.UsuarioId_Usuario,
                        principalTable: "Usuarios",
                        principalColumn: "Id_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_UsuarioCreaId_Usuario",
                table: "Auditoria",
                column: "UsuarioCreaId_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_UsuarioModificaId_Usuario",
                table: "Auditoria",
                column: "UsuarioModificaId_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Dominios_PadreId_Dominio",
                table: "Dominios",
                column: "PadreId_Dominio");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_PermisoId_Permiso",
                table: "Roles_Permisos",
                column: "PermisoId_Permiso");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_RolId_Rol",
                table: "Roles_Permisos",
                column: "RolId_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_RolId_Rol",
                table: "Roles_User",
                column: "RolId_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_UsuarioId_Usuario",
                table: "Roles_User",
                column: "UsuarioId_Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auditoria");

            migrationBuilder.DropTable(
                name: "Dominios");

            migrationBuilder.DropTable(
                name: "Roles_Permisos");

            migrationBuilder.DropTable(
                name: "Roles_User");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
