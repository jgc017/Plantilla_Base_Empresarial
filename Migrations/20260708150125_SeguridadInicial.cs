using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Plantilla_Base.Migrations
{
    /// <inheritdoc />
    public partial class SeguridadInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditoria_Usuarios_UsuarioCreaId_Usuario",
                table: "Auditoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Auditoria_Usuarios_UsuarioModificaId_Usuario",
                table: "Auditoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Dominios_Dominios_PadreId_Dominio",
                table: "Dominios");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permisos_Permisos_PermisoId_Permiso",
                table: "Roles_Permisos");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permisos_Roles_RolId_Rol",
                table: "Roles_Permisos");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_User_Roles_RolId_Rol",
                table: "Roles_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_User_Usuarios_UsuarioId_Usuario",
                table: "Roles_User");

            migrationBuilder.DropIndex(
                name: "IX_Roles_User_RolId_Rol",
                table: "Roles_User");

            migrationBuilder.DropIndex(
                name: "IX_Roles_User_UsuarioId_Usuario",
                table: "Roles_User");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Permisos_PermisoId_Permiso",
                table: "Roles_Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Permisos_RolId_Rol",
                table: "Roles_Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Dominios_PadreId_Dominio",
                table: "Dominios");

            migrationBuilder.DropIndex(
                name: "IX_Auditoria_UsuarioCreaId_Usuario",
                table: "Auditoria");

            migrationBuilder.DropIndex(
                name: "IX_Auditoria_UsuarioModificaId_Usuario",
                table: "Auditoria");

            migrationBuilder.DropColumn(
                name: "RolId_Rol",
                table: "Roles_User");

            migrationBuilder.DropColumn(
                name: "UsuarioId_Usuario",
                table: "Roles_User");

            migrationBuilder.DropColumn(
                name: "PermisoId_Permiso",
                table: "Roles_Permisos");

            migrationBuilder.DropColumn(
                name: "RolId_Rol",
                table: "Roles_Permisos");

            migrationBuilder.DropColumn(
                name: "PadreId_Dominio",
                table: "Dominios");

            migrationBuilder.DropColumn(
                name: "UsuarioCreaId_Usuario",
                table: "Auditoria");

            migrationBuilder.DropColumn(
                name: "UsuarioModificaId_Usuario",
                table: "Auditoria");

            migrationBuilder.AlterColumn<string>(
                name: "Usuario",
                table: "Usuarios",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "E_Mail",
                table: "Usuarios",
                type: "character varying(160)",
                maxLength: 160,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Roles",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Modulo",
                table: "Permisos",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Permisos",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Accion",
                table: "Permisos",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Observacion",
                table: "Dominios",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Dominios",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Auditoria",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_E_Mail",
                table: "Usuarios",
                column: "E_Mail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Usuario",
                table: "Usuarios",
                column: "Usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_Id_Rol",
                table: "Roles_User",
                column: "Id_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_Id_Usuario_Id_Rol",
                table: "Roles_User",
                columns: new[] { "Id_Usuario", "Id_Rol" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_Id_Permiso",
                table: "Roles_Permisos",
                column: "Id_Permiso");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_Id_Rol_Id_Permiso",
                table: "Roles_Permisos",
                columns: new[] { "Id_Rol", "Id_Permiso" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Rol",
                table: "Roles",
                column: "Rol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos",
                columns: new[] { "Modulo", "Accion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dominios_Id_Padre",
                table: "Dominios",
                column: "Id_Padre");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_Id_Usuario_Creacion",
                table: "Auditoria",
                column: "Id_Usuario_Creacion");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_Id_Usuario_Modifica",
                table: "Auditoria",
                column: "Id_Usuario_Modifica");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditoria_Usuarios_Id_Usuario_Creacion",
                table: "Auditoria",
                column: "Id_Usuario_Creacion",
                principalTable: "Usuarios",
                principalColumn: "Id_Usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Auditoria_Usuarios_Id_Usuario_Modifica",
                table: "Auditoria",
                column: "Id_Usuario_Modifica",
                principalTable: "Usuarios",
                principalColumn: "Id_Usuario",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dominios_Dominios_Id_Padre",
                table: "Dominios",
                column: "Id_Padre",
                principalTable: "Dominios",
                principalColumn: "Id_Dominio",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permisos_Permisos_Id_Permiso",
                table: "Roles_Permisos",
                column: "Id_Permiso",
                principalTable: "Permisos",
                principalColumn: "Id_Permiso",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permisos_Roles_Id_Rol",
                table: "Roles_Permisos",
                column: "Id_Rol",
                principalTable: "Roles",
                principalColumn: "Id_Rol",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_User_Roles_Id_Rol",
                table: "Roles_User",
                column: "Id_Rol",
                principalTable: "Roles",
                principalColumn: "Id_Rol",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_User_Usuarios_Id_Usuario",
                table: "Roles_User",
                column: "Id_Usuario",
                principalTable: "Usuarios",
                principalColumn: "Id_Usuario",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auditoria_Usuarios_Id_Usuario_Creacion",
                table: "Auditoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Auditoria_Usuarios_Id_Usuario_Modifica",
                table: "Auditoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Dominios_Dominios_Id_Padre",
                table: "Dominios");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permisos_Permisos_Id_Permiso",
                table: "Roles_Permisos");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Permisos_Roles_Id_Rol",
                table: "Roles_Permisos");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_User_Roles_Id_Rol",
                table: "Roles_User");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_User_Usuarios_Id_Usuario",
                table: "Roles_User");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_E_Mail",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Usuario",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Roles_User_Id_Rol",
                table: "Roles_User");

            migrationBuilder.DropIndex(
                name: "IX_Roles_User_Id_Usuario_Id_Rol",
                table: "Roles_User");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Permisos_Id_Permiso",
                table: "Roles_Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Permisos_Id_Rol_Id_Permiso",
                table: "Roles_Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Rol",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Permisos_Modulo_Accion",
                table: "Permisos");

            migrationBuilder.DropIndex(
                name: "IX_Dominios_Id_Padre",
                table: "Dominios");

            migrationBuilder.DropIndex(
                name: "IX_Auditoria_Id_Usuario_Creacion",
                table: "Auditoria");

            migrationBuilder.DropIndex(
                name: "IX_Auditoria_Id_Usuario_Modifica",
                table: "Auditoria");

            migrationBuilder.AlterColumn<string>(
                name: "Usuario",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "E_Mail",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(160)",
                oldMaxLength: 160,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RolId_Rol",
                table: "Roles_User",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId_Usuario",
                table: "Roles_User",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PermisoId_Permiso",
                table: "Roles_Permisos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RolId_Rol",
                table: "Roles_Permisos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Roles",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Modulo",
                table: "Permisos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Permisos",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Accion",
                table: "Permisos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(80)",
                oldMaxLength: 80);

            migrationBuilder.AlterColumn<string>(
                name: "Observacion",
                table: "Dominios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Dominios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AddColumn<int>(
                name: "PadreId_Dominio",
                table: "Dominios",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descripcion",
                table: "Auditoria",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioCreaId_Usuario",
                table: "Auditoria",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsuarioModificaId_Usuario",
                table: "Auditoria",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_RolId_Rol",
                table: "Roles_User",
                column: "RolId_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_User_UsuarioId_Usuario",
                table: "Roles_User",
                column: "UsuarioId_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_PermisoId_Permiso",
                table: "Roles_Permisos",
                column: "PermisoId_Permiso");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Permisos_RolId_Rol",
                table: "Roles_Permisos",
                column: "RolId_Rol");

            migrationBuilder.CreateIndex(
                name: "IX_Dominios_PadreId_Dominio",
                table: "Dominios",
                column: "PadreId_Dominio");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_UsuarioCreaId_Usuario",
                table: "Auditoria",
                column: "UsuarioCreaId_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Auditoria_UsuarioModificaId_Usuario",
                table: "Auditoria",
                column: "UsuarioModificaId_Usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditoria_Usuarios_UsuarioCreaId_Usuario",
                table: "Auditoria",
                column: "UsuarioCreaId_Usuario",
                principalTable: "Usuarios",
                principalColumn: "Id_Usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Auditoria_Usuarios_UsuarioModificaId_Usuario",
                table: "Auditoria",
                column: "UsuarioModificaId_Usuario",
                principalTable: "Usuarios",
                principalColumn: "Id_Usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_Dominios_Dominios_PadreId_Dominio",
                table: "Dominios",
                column: "PadreId_Dominio",
                principalTable: "Dominios",
                principalColumn: "Id_Dominio");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permisos_Permisos_PermisoId_Permiso",
                table: "Roles_Permisos",
                column: "PermisoId_Permiso",
                principalTable: "Permisos",
                principalColumn: "Id_Permiso",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Permisos_Roles_RolId_Rol",
                table: "Roles_Permisos",
                column: "RolId_Rol",
                principalTable: "Roles",
                principalColumn: "Id_Rol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_User_Roles_RolId_Rol",
                table: "Roles_User",
                column: "RolId_Rol",
                principalTable: "Roles",
                principalColumn: "Id_Rol",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_User_Usuarios_UsuarioId_Usuario",
                table: "Roles_User",
                column: "UsuarioId_Usuario",
                principalTable: "Usuarios",
                principalColumn: "Id_Usuario",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
