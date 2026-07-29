using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Plantilla_Base.Data;

#nullable disable

namespace Plantilla_Base.Migrations
{
    // Relaciona permisos con menus para que la autorizacion no dependa de textos.
    [DbContext(typeof(AppDbContext))]
    [Migration("20260718140000_AgregaIdMenuAPermisos")]
    public partial class AgregaIdMenuAPermisos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Menu",
                table: "Permisos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Id_Menu_Accion",
                table: "Permisos",
                columns: new[] { "Id_Menu", "Accion" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_Menus_Id_Menu",
                table: "Permisos",
                column: "Id_Menu",
                principalTable: "Menus",
                principalColumn: "Id_Menu",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_Menus_Id_Menu",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_Id_Menu_Accion",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Id_Menu",
                table: "Permisos");
        }
    }
}
