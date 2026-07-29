using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class AgregaIdentificacionYSolicitudRestauracionContrasena : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Identificacion",
                table: "Usuarios",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Usuarios",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Auditoria",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Identificacion",
                table: "Usuarios",
                column: "Identificacion",
                unique: true,
                filter: "\"Identificacion\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Restaurar_Contrasena_Id_Usuario_Atiende",
                table: "Solicitud_Restaurar_Contrasena",
                column: "Id_Usuario_Atiende");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitud_Restaurar_Contrasena_Id_Usuario_Estado_Vigente",
                table: "Solicitud_Restaurar_Contrasena",
                columns: new[] { "Id_Usuario", "Estado", "Vigente" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitud_Restaurar_Contrasena");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Identificacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Identificacion",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Usuarios");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Auditoria",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(4000)",
                oldMaxLength: 4000,
                oldNullable: true);
        }
    }
}
