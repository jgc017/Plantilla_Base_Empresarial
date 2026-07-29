using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class RecuperacionPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_Id_Usuario_Fecha_Expiracion",
                table: "PasswordResetTokens",
                columns: new[] { "Id_Usuario", "Fecha_Expiracion" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_TokenHash",
                table: "PasswordResetTokens",
                column: "TokenHash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PasswordResetTokens");
        }
    }
}
