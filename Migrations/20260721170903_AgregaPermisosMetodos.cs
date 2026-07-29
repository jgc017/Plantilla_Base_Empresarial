using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class AgregaPermisosMetodos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permisos_Id_Menu_Accion",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos");

            migrationBuilder.AddColumn<string>(
                name: "CodigoPermiso",
                table: "Permisos",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Controlador",
                table: "Permisos",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HttpMetodo",
                table: "Permisos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metodo",
                table: "Permisos",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPermiso",
                table: "Permisos",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Menu");

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
                filter: "\"Id_Menu\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Permisos_CodigoPermiso",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_Id_Menu",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_TipoPermiso_Id_Menu_Accion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "CodigoPermiso",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Controlador",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "HttpMetodo",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Metodo",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "TipoPermiso",
                table: "Permisos");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Id_Menu_Accion",
                table: "Permisos",
                columns: new[] { "Id_Menu", "Accion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos",
                columns: new[] { "Modulo", "Accion" },
                unique: true);
        }
    }
}
