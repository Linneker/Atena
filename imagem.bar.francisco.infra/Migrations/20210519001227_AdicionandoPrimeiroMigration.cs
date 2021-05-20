using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace imagem.bar.francisco.infra.Migrations
{
    public partial class AdicionandoPrimeiroMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Competencia",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Despesa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    DespesaFixa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Despesa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Despesa_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    SaldoOperacional = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SaldoFinalCaixa = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoDeCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixa_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Receita",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ReceitaFixa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receita_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixaDespesa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    FluxoDeCaixaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DespesaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoDeCaixaDespesa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaDespesa_Despesa_DespesaId",
                        column: x => x.DespesaId,
                        principalTable: "Despesa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaDespesa_FluxoDeCaixa_FluxoDeCaixaId",
                        column: x => x.FluxoDeCaixaId,
                        principalTable: "FluxoDeCaixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixaReceita",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    FluxoDeCaixaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ReceitaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoDeCaixaReceita", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaReceita_FluxoDeCaixa_FluxoDeCaixaId",
                        column: x => x.FluxoDeCaixaId,
                        principalTable: "FluxoDeCaixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaReceita_Receita_ReceitaId",
                        column: x => x.ReceitaId,
                        principalTable: "Receita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_CompetenciaId",
                table: "Despesa",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoDeCaixa_CompetenciaId",
                table: "FluxoDeCaixa",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoDeCaixaDespesa_DespesaId",
                table: "FluxoDeCaixaDespesa",
                column: "DespesaId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoDeCaixaDespesa_FluxoDeCaixaId",
                table: "FluxoDeCaixaDespesa",
                column: "FluxoDeCaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoDeCaixaReceita_FluxoDeCaixaId",
                table: "FluxoDeCaixaReceita",
                column: "FluxoDeCaixaId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoDeCaixaReceita_ReceitaId",
                table: "FluxoDeCaixaReceita",
                column: "ReceitaId");

            migrationBuilder.CreateIndex(
                name: "IX_Receita_CompetenciaId",
                table: "Receita",
                column: "CompetenciaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FluxoDeCaixaDespesa");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixaReceita");

            migrationBuilder.DropTable(
                name: "Despesa");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixa");

            migrationBuilder.DropTable(
                name: "Receita");

            migrationBuilder.DropTable(
                name: "Competencia");
        }
    }
}
