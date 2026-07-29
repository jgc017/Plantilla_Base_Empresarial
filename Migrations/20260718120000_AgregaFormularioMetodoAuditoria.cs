using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Plantilla_Base.Data;

#nullable disable

namespace Plantilla_Base.Migrations
{
    // Agrega datos de contexto funcional a la tabla Auditoria.
    [DbContext(typeof(AppDbContext))]
    [Migration("20260718120000_AgregaFormularioMetodoAuditoria")]
    public partial class AgregaFormularioMetodoAuditoria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Formulario",
                table: "Auditoria",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metodo_Ejecutado",
                table: "Auditoria",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formulario",
                table: "Auditoria");

            migrationBuilder.DropColumn(
                name: "Metodo_Ejecutado",
                table: "Auditoria");
        }
    }
}
