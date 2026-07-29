using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Plantilla_Base.Data;

#nullable disable

namespace Plantilla_Base.Migrations
{
    // Crea la tabla Menus para administrar las opciones visibles del sistema.
    [DbContext(typeof(AppDbContext))]
    [Migration("20260718130000_CreaTablaMenus")]
    public partial class CreaTablaMenus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Id_Padre",
                table: "Menus",
                column: "Id_Padre");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Controlador_Vista",
                table: "Menus",
                columns: new[] { "Controlador", "Vista" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Menus");
        }
    }
}
