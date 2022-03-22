using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acme.atena.infra.Migrations
{
    public partial class modificandoPermissaoUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Add",
                table: "PermissaoUsuario");

            migrationBuilder.DropColumn(
                name: "Delete",
                table: "PermissaoUsuario");

            migrationBuilder.DropColumn(
                name: "Read",
                table: "PermissaoUsuario");

            migrationBuilder.DropColumn(
                name: "Update",
                table: "PermissaoUsuario");

            migrationBuilder.AddColumn<string>(
                name: "Acesso",
                table: "PermissaoUsuario",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "TelaId",
                table: "PermissaoUsuario",
                type: "varbinary(16)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tela",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Icon = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Title = table.Column<bool>(type: "tinyint(255)", maxLength: 255, nullable: true),
                    IsPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Class = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Variant = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TelaSistemaFilhaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tela", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tela_Tela_TelaSistemaFilhaId",
                        column: x => x.TelaSistemaFilhaId,
                        principalTable: "Tela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoUsuario_TelaId",
                table: "PermissaoUsuario",
                column: "TelaId");

            migrationBuilder.CreateIndex(
                name: "IX_Tela_TelaSistemaFilhaId",
                table: "Tela",
                column: "TelaSistemaFilhaId");

            migrationBuilder.AddForeignKey(
                name: "FK_PermissaoUsuario_Tela_TelaId",
                table: "PermissaoUsuario",
                column: "TelaId",
                principalTable: "Tela",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PermissaoUsuario_Tela_TelaId",
                table: "PermissaoUsuario");

            migrationBuilder.DropTable(
                name: "Tela");

            migrationBuilder.DropIndex(
                name: "IX_PermissaoUsuario_TelaId",
                table: "PermissaoUsuario");

            migrationBuilder.DropColumn(
                name: "Acesso",
                table: "PermissaoUsuario");

            migrationBuilder.DropColumn(
                name: "TelaId",
                table: "PermissaoUsuario");

            migrationBuilder.AddColumn<bool>(
                name: "Add",
                table: "PermissaoUsuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Delete",
                table: "PermissaoUsuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Read",
                table: "PermissaoUsuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "Update",
                table: "PermissaoUsuario",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
