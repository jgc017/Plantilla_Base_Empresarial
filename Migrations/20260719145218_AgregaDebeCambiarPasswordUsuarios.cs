using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class AgregaDebeCambiarPasswordUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Debe_Cambiar_Password",
                table: "Usuarios",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Debe_Cambiar_Password",
                table: "Usuarios");
        }
    }
}
