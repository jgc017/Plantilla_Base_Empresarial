using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Plantilla_Base.Data;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AppDbContext))]
    [Migration("20260716180000_AgregaDominioPadreADominios")]
    public partial class AgregaDominioPadreADominios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DominioPadre",
                table: "Dominios",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "No");

            migrationBuilder.Sql("UPDATE \"Dominios\" SET \"DominioPadre\" = 'Si';");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DominioPadre",
                table: "Dominios");
        }
    }
}
