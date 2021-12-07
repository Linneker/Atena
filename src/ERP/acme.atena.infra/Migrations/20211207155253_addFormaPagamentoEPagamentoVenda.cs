using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acme.atena.infra.Migrations
{
    public partial class addFormaPagamentoEPagamentoVenda : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Cliente_PessoaId",
                table: "Venda");

            migrationBuilder.DropIndex(
                name: "IX_Venda_PessoaId",
                table: "Venda");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "Venda");

            migrationBuilder.DropColumn(
                name: "EnumFormaPagamento",
                table: "Pagamento");

            migrationBuilder.AddColumn<byte[]>(
                name: "ClienteId",
                table: "Venda",
                type: "varbinary(16)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "EmpresaId",
                table: "Venda",
                type: "varbinary(16)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PagamentoVendaId",
                table: "Pagamento",
                type: "varbinary(16)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FormaPagamento",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaPagamento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PagamentoVenda",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false),
                    Parcelado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NumeroDeParcela = table.Column<int>(type: "int", nullable: false),
                    DiaVencimentoParcela = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DataPagamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataCredito = table.Column<DateTime>(type: "datetime", nullable: false),
                    VendaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentoVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentoVenda_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagamentoVenda_Venda_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Venda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagamentoFormaPagamento",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    PagamentoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    FormaPagamentoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentoFormaPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentoFormaPagamento_FormaPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormaPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagamentoFormaPagamento_Pagamento_PagamentoId",
                        column: x => x.PagamentoId,
                        principalTable: "Pagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PagamentoVendaFormaPagamento",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    PagamentoVendaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    FormaPagamentoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagamentoVendaFormaPagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PagamentoVendaFormaPagamento_FormaPagamento_FormaPagamentoId",
                        column: x => x.FormaPagamentoId,
                        principalTable: "FormaPagamento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PagamentoVendaFormaPagamento_PagamentoVenda_PagamentoVendaId",
                        column: x => x.PagamentoVendaId,
                        principalTable: "PagamentoVenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Venda_ClienteId",
                table: "Venda",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_EmpresaId",
                table: "Venda",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_PagamentoVendaId",
                table: "Pagamento",
                column: "PagamentoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoFormaPagamento_FormaPagamentoId",
                table: "PagamentoFormaPagamento",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoFormaPagamento_PagamentoId",
                table: "PagamentoFormaPagamento",
                column: "PagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoVenda_CompetenciaId",
                table: "PagamentoVenda",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoVenda_VendaId",
                table: "PagamentoVenda",
                column: "VendaId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoVendaFormaPagamento_FormaPagamentoId",
                table: "PagamentoVendaFormaPagamento",
                column: "FormaPagamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_PagamentoVendaFormaPagamento_PagamentoVendaId",
                table: "PagamentoVendaFormaPagamento",
                column: "PagamentoVendaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamento_PagamentoVenda_PagamentoVendaId",
                table: "Pagamento",
                column: "PagamentoVendaId",
                principalTable: "PagamentoVenda",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Cliente_ClienteId",
                table: "Venda",
                column: "ClienteId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Empresa_EmpresaId",
                table: "Venda",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagamento_PagamentoVenda_PagamentoVendaId",
                table: "Pagamento");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Cliente_ClienteId",
                table: "Venda");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Empresa_EmpresaId",
                table: "Venda");

            migrationBuilder.DropTable(
                name: "PagamentoFormaPagamento");

            migrationBuilder.DropTable(
                name: "PagamentoVendaFormaPagamento");

            migrationBuilder.DropTable(
                name: "FormaPagamento");

            migrationBuilder.DropTable(
                name: "PagamentoVenda");

            migrationBuilder.DropIndex(
                name: "IX_Venda_ClienteId",
                table: "Venda");

            migrationBuilder.DropIndex(
                name: "IX_Venda_EmpresaId",
                table: "Venda");

            migrationBuilder.DropIndex(
                name: "IX_Pagamento_PagamentoVendaId",
                table: "Pagamento");

            migrationBuilder.DropColumn(
                name: "ClienteId",
                table: "Venda");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Venda");

            migrationBuilder.DropColumn(
                name: "PagamentoVendaId",
                table: "Pagamento");

            migrationBuilder.AddColumn<byte[]>(
                name: "PessoaId",
                table: "Venda",
                type: "varbinary(16)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<int>(
                name: "EnumFormaPagamento",
                table: "Pagamento",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Venda_PessoaId",
                table: "Venda",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Cliente_PessoaId",
                table: "Venda",
                column: "PessoaId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
