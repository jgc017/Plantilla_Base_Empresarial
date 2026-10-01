using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class AddMotivos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Usuarios",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "SistemaVisualConfig",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "SistemaVisualConfig",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Roles_User",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Roles_User",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Roles_Permisos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Roles_Permisos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Roles",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Roles",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Permisos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Permisos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Menus",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Menus",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "InicioContenidos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "InicioContenidos",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Actualiza",
                table: "Dominios",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motivo_Elimina",
                table: "Dominios",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "SistemaVisualConfig");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "SistemaVisualConfig");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Roles_User");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Roles_User");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Roles_Permisos");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Roles_Permisos");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Menus");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "InicioContenidos");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "InicioContenidos");

            migrationBuilder.DropColumn(
                name: "Motivo_Actualiza",
                table: "Dominios");

            migrationBuilder.DropColumn(
                name: "Motivo_Elimina",
                table: "Dominios");
        }
    }
}
