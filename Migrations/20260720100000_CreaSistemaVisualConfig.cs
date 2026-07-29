using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Plantilla_Base.Data;

#nullable disable

namespace Plantilla_Base.Migrations
{
    // Crea la tabla de configuracion visual global del sistema.
    [DbContext(typeof(AppDbContext))]
    [Migration("20260720100000_CreaSistemaVisualConfig")]
    public partial class CreaSistemaVisualConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SistemaVisualConfig",
                columns: table => new
                {
                    Id_SistemaVisualConfig = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FaviconUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LoginBackgroundUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
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
                    table.PrimaryKey("PK_SistemaVisualConfig", x => x.Id_SistemaVisualConfig);
                });

            migrationBuilder.Sql("""
                INSERT INTO "SistemaVisualConfig" (
                    "LogoUrl",
                    "FaviconUrl",
                    "LoginBackgroundUrl",
                    "Vigente",
                    "Id_Usuario_Creacion",
                    "Fecha_Creacion",
                    "Maquina_Creacion"
                )
                VALUES (
                    '/img/IMAGENIA.png',
                    '/favicon.ico',
                    '/img/auth-background.svg',
                    1,
                    1,
                    now(),
                    'Migracion'
                );
                """);

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
                    'Configuracion visual',
                    admin."Id_Menu",
                    COALESCE((SELECT MAX(hijo."Posicion") + 1 FROM "Menus" hijo WHERE hijo."Id_Padre" = admin."Id_Menu"), 1),
                    'Formulario',
                    'SistemaConfig',
                    'VwSistemaConfig',
                    'fa-solid fa-image',
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
                        WHERE existente."Controlador" = 'SistemaConfig'
                            AND existente."Vista" = 'VwSistemaConfig'
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DELETE FROM "Menus"
                WHERE "Controlador" = 'SistemaConfig'
                    AND "Vista" = 'VwSistemaConfig';
                """);

            migrationBuilder.DropTable(
                name: "SistemaVisualConfig");
        }
    }
}
