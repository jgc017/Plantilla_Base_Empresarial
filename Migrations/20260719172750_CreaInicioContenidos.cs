using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class CreaInicioContenidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InicioContenidos",
                columns: table => new
                {
                    Id_InicioContenido = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoContenido = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    Resumen = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Contenido = table.Column<string>(type: "text", nullable: true),
                    ImagenUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EnlaceUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TextoBoton = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    MostrarEnInicio = table.Column<short>(type: "smallint", nullable: false),
                    Orden = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_InicioContenidos", x => x.Id_InicioContenido);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InicioContenidos_TipoContenido_Orden",
                table: "InicioContenidos",
                columns: new[] { "TipoContenido", "Orden" });

            migrationBuilder.Sql("""
                INSERT INTO "Menus" (
                    "Id_Menu",
                    "Descripcion",
                    "Id_Padre",
                    "Posicion",
                    "Tipo",
                    "Controlador",
                    "Vista",
                    "Icono",
                    "Vigente",
                    "Id_Usuario_Creacion",
                    "Fecha_Creacion",
                    "Maquina_Creacion"
                )
                SELECT
                    COALESCE((SELECT MAX(menu."Id_Menu") + 1 FROM "Menus" menu), 1),
                    'Inicio Admin',
                    admin."Id_Menu",
                    COALESCE((SELECT MAX(hijo."Posicion") + 1 FROM "Menus" hijo WHERE hijo."Id_Padre" = admin."Id_Menu"), 1),
                    'Formulario',
                    'InicioAdmin',
                    'VwInicioAdmin',
                    'fa-solid fa-house-chimney-window',
                    1,
                    1,
                    now(),
                    'Migracion'
                FROM "Menus" admin
                WHERE (admin."Id_Menu" = 2 OR admin."Descripcion" IN ('Administracion', 'Administración'))
                    AND admin."Id_Padre" IS NULL
                    AND NOT EXISTS (
                        SELECT 1
                        FROM "Menus" existente
                        WHERE existente."Controlador" = 'InicioAdmin'
                            AND existente."Vista" = 'VwInicioAdmin'
                    );
                """);

            migrationBuilder.Sql("""
                SELECT setval(
                    pg_get_serial_sequence('"Menus"', 'Id_Menu'),
                    COALESCE((SELECT MAX("Id_Menu") FROM "Menus"), 1),
                    true
                );
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM "Menus"
                WHERE "Controlador" = 'InicioAdmin'
                    AND "Vista" = 'VwInicioAdmin';
                """);

            migrationBuilder.DropTable(
                name: "InicioContenidos");
        }
    }
}
