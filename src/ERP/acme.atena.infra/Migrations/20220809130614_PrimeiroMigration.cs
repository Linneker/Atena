using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace acme.atena.infra.Migrations
{
    public partial class PrimeiroMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AutorizacaoApi",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AccessKey = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacaoApi", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NomeFantasia = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InscricaoMunicipal = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Competencia",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competencia", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsFilial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmpresaMatrizId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    RazaoSocial = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresa_Empresa_EmpresaMatrizId",
                        column: x => x.EmpresaMatrizId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Cep = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pais = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Bairro = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rua = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FormaPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormaPagamento", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    NomeFantasia = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InscricaoMunicipal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Parametro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<string>(type: "varchar(1500)", unicode: false, maxLength: 1500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExibirTela = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametro", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permissao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(110)", unicode: false, maxLength: 110, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissao", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sequence_Codigo_Produto",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequence_Codigo_Produto", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tela",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Icon = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<bool>(type: "tinyint(255)", maxLength: 255, nullable: true),
                    IsPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Class = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Variant = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelaSistemaFilhaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoDocumento", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProduto", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoValorProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoValorProduto", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Login = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Senha = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TermoDeAceite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Despesa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    DespesaFixa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FluxoCaixaEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ValorTotalEstoqueInicial = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    ValorTotalEntrada = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    ValorTotalSaida = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    ValorTotalEstoqueFinal = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxoCaixaEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxoCaixaEstoque_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SaldoOperacional = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    SaldoFinalCaixa = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Receita",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Valor = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    ReceitaFixa = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    EstoqueMaximo = table.Column<int>(type: "int", nullable: false),
                    EstoqueMinimo = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estoque_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnderecoCliente",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClienteId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnderecoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoCliente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoCliente_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnderecoCliente_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnderecoEmpresa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EmpresaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnderecoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoEmpresa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoEmpresa_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnderecoEmpresa_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnderecoFornecedor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnderecoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoFornecedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoFornecedor_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnderecoFornecedor_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Token = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Container = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hash = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoDocumentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documento_TipoDocumento_TipoDocumentoId",
                        column: x => x.TipoDocumentoId,
                        principalTable: "TipoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<long>(type: "bigint", nullable: false),
                    Validade = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TipoProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_Sequence_Codigo_Produto_Codigo",
                        column: x => x.Codigo,
                        principalTable: "Sequence_Codigo_Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produto_TipoProduto_TipoProdutoId",
                        column: x => x.TipoProdutoId,
                        principalTable: "TipoProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCompra = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValorTotal = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compra_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Compra_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Divida",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnumTipoDivida = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    DataQueDeviaTerRecebido = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ClienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EmpresaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divida", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Divida_Cliente_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Divida_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Divida_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Divida_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Divida_Usuario_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EnderecoUsuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnderecoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnderecoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoUsuario_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnderecoUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PermissaoUsuario",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Acesso = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TelaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PermissaoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissaoUsuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissaoUsuario_Permissao_PermissaoId",
                        column: x => x.PermissaoId,
                        principalTable: "Permissao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissaoUsuario_Tela_TelaId",
                        column: x => x.TelaId,
                        principalTable: "Tela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissaoUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Venda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataVenda = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    ValorPago = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: true),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EmpresaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venda_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Venda_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Venda_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Venda_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixaDespesa",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FluxoDeCaixaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DespesaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaDespesa_FluxoDeCaixa_FluxoDeCaixaId",
                        column: x => x.FluxoDeCaixaId,
                        principalTable: "FluxoDeCaixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixaReceita",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FluxoDeCaixaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ReceitaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaReceita_Receita_ReceitaId",
                        column: x => x.ReceitaId,
                        principalTable: "Receita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntradaProdutoEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EstoqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NumeroNotaFiscal = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustoUnitario = table.Column<decimal>(type: "decimal(32,2)", precision: 32, scale: 2, nullable: false),
                    QuantidadeTotal = table.Column<long>(type: "bigint", nullable: false),
                    PrazoEntradaDiasUteis = table.Column<int>(type: "int", nullable: false),
                    EstoqueEstimado = table.Column<long>(type: "bigint", nullable: false),
                    TotalCompras = table.Column<decimal>(type: "decimal(32,2)", precision: 32, scale: 2, nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaProdutoEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoque_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoque_Estoque_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoque_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EstoqueProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EstoqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QuantidadeMaxima = table.Column<long>(type: "bigint", nullable: false),
                    QuantidadeMinima = table.Column<long>(type: "bigint", nullable: false),
                    QuantidadeProduto = table.Column<long>(type: "bigint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstoqueProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EstoqueProduto_Estoque_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EstoqueProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaidaProdutoEstoque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EstoqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaidaProdutoEstoque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaidaProdutoEstoque_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaidaProdutoEstoque_Estoque_EstoqueId",
                        column: x => x.EstoqueId,
                        principalTable: "Estoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaidaProdutoEstoque_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ValorProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Valor = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TipoValorProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValorProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValorProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValorProduto_TipoValorProduto_TipoValorProdutoId",
                        column: x => x.TipoValorProdutoId,
                        principalTable: "TipoValorProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompraProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Valor = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    QuantidadeComprada = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsRecebido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    CompraId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompraProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompraProduto_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompraProduto_Compra_CompraId",
                        column: x => x.CompraId,
                        principalTable: "Compra",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompraProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PagamentoVenda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    StatusPagamento = table.Column<int>(type: "int", nullable: false),
                    Parcelado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NumeroDeParcela = table.Column<int>(type: "int", nullable: false),
                    DiaVencimentoParcela = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataCredito = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VendaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VendaProduto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Valor = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    QuantidadeVedida = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Enviado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VendaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendaProduto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendaProduto_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendaProduto_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VendaProduto_Venda_VendaId",
                        column: x => x.VendaId,
                        principalTable: "Venda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntradaProdutoEstoqueDocumento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EntradaProdutoEstoqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DocumentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaProdutoEstoqueDocumento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoqueDocumento_Documento_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoqueDocumento_EntradaProdutoEstoque_Entrada~",
                        column: x => x.EntradaProdutoEstoqueId,
                        principalTable: "EntradaProdutoEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EntradaProdutoEstoqueFornecedor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EntradaProdutoEstoqueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntradaProdutoEstoqueFornecedor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoqueFornecedor_EntradaProdutoEstoque_Entrad~",
                        column: x => x.EntradaProdutoEstoqueId,
                        principalTable: "EntradaProdutoEstoque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EntradaProdutoEstoqueFornecedor_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DevolucaoCompra",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Motivo = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CompraProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsParcial = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevolucaoCompra", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevolucaoCompra_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevolucaoCompra_CompraProduto_CompraProdutoId",
                        column: x => x.CompraProdutoId,
                        principalTable: "CompraProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevolucaoCompra_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Parcelado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    NumeroDaParcela = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DataPagamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataCredito = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValorPago = table.Column<decimal>(type: "decimal(24,4)", precision: 24, scale: 4, nullable: false),
                    EnumTipoPagamento = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    FornecedorId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ClienteId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    EmpresaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    PagamentoVendaId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagamento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagamento_Cliente_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_Empresa_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "Empresa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_Fornecedor_FornecedorId",
                        column: x => x.FornecedorId,
                        principalTable: "Fornecedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_PagamentoVenda_PagamentoVendaId",
                        column: x => x.PagamentoVendaId,
                        principalTable: "PagamentoVenda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagamento_Usuario_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PagamentoVendaFormaPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PagamentoVendaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FormaPagamentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DevolucaoVenda",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Motivo = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VendaProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProdutoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsParcial = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevolucaoVenda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevolucaoVenda_Competencia_CompetenciaId",
                        column: x => x.CompetenciaId,
                        principalTable: "Competencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevolucaoVenda_Produto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Produto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DevolucaoVenda_VendaProduto_VendaProdutoId",
                        column: x => x.VendaProdutoId,
                        principalTable: "VendaProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PagamentoFormaPagamento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PagamentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    FormaPagamentoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataCriacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    UsuarioCriacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    UsuarioModificacao = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CpfCnpj",
                table: "Cliente",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Email",
                table: "Cliente",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Compra_CompetenciaId",
                table: "Compra",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_FornecedorId",
                table: "Compra",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Compra_UsuarioId",
                table: "Compra",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraProduto_CompetenciaId",
                table: "CompraProduto",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraProduto_CompraId",
                table: "CompraProduto",
                column: "CompraId");

            migrationBuilder.CreateIndex(
                name: "IX_CompraProduto_ProdutoId",
                table: "CompraProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Despesa_CompetenciaId",
                table: "Despesa",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucaoCompra_CompetenciaId",
                table: "DevolucaoCompra",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucaoCompra_CompraProdutoId",
                table: "DevolucaoCompra",
                column: "CompraProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucaoCompra_ProdutoId",
                table: "DevolucaoCompra",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucaoVenda_CompetenciaId",
                table: "DevolucaoVenda",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucaoVenda_ProdutoId",
                table: "DevolucaoVenda",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_DevolucaoVenda_VendaProdutoId",
                table: "DevolucaoVenda",
                column: "VendaProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Divida_ClienteId",
                table: "Divida",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Divida_CompetenciaId",
                table: "Divida",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Divida_EmpresaId",
                table: "Divida",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Divida_FornecedorId",
                table: "Divida",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Divida_UsuarioId",
                table: "Divida",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Documento_TipoDocumentoId",
                table: "Documento",
                column: "TipoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_CpfCnpj",
                table: "Empresa",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_Email",
                table: "Empresa",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_EmpresaMatrizId",
                table: "Empresa",
                column: "EmpresaMatrizId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoCliente_ClienteId",
                table: "EnderecoCliente",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoCliente_EnderecoId",
                table: "EnderecoCliente",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoEmpresa_EmpresaId",
                table: "EnderecoEmpresa",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoEmpresa_EnderecoId",
                table: "EnderecoEmpresa",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoFornecedor_EnderecoId",
                table: "EnderecoFornecedor",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoFornecedor_FornecedorId",
                table: "EnderecoFornecedor",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoUsuario_EnderecoId",
                table: "EnderecoUsuario",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoUsuario_UsuarioId",
                table: "EnderecoUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoque_CompetenciaId",
                table: "EntradaProdutoEstoque",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoque_EstoqueId",
                table: "EntradaProdutoEstoque",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoque_ProdutoId",
                table: "EntradaProdutoEstoque",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoqueDocumento_DocumentoId",
                table: "EntradaProdutoEstoqueDocumento",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoqueDocumento_EntradaProdutoEstoqueId",
                table: "EntradaProdutoEstoqueDocumento",
                column: "EntradaProdutoEstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoqueFornecedor_EntradaProdutoEstoqueId",
                table: "EntradaProdutoEstoqueFornecedor",
                column: "EntradaProdutoEstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EntradaProdutoEstoqueFornecedor_FornecedorId",
                table: "EntradaProdutoEstoqueFornecedor",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_EmpresaId",
                table: "Estoque",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueProduto_EstoqueId",
                table: "EstoqueProduto",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_EstoqueProduto_ProdutoId",
                table: "EstoqueProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_FluxoCaixaEstoque_CompetenciaId",
                table: "FluxoCaixaEstoque",
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
                name: "IX_Fornecedor_CpfCnpj",
                table: "Fornecedor",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fornecedor_Email",
                table: "Fornecedor",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_ClienteId",
                table: "Pagamento",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_CompetenciaId",
                table: "Pagamento",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_EmpresaId",
                table: "Pagamento",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_FornecedorId",
                table: "Pagamento",
                column: "FornecedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_PagamentoVendaId",
                table: "Pagamento",
                column: "PagamentoVendaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamento_UsuarioId",
                table: "Pagamento",
                column: "UsuarioId");

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

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoUsuario_PermissaoId",
                table: "PermissaoUsuario",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoUsuario_TelaId",
                table: "PermissaoUsuario",
                column: "TelaId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoUsuario_UsuarioId",
                table: "PermissaoUsuario",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_Codigo",
                table: "Produto",
                column: "Codigo");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_TipoProdutoId",
                table: "Produto",
                column: "TipoProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Receita_CompetenciaId",
                table: "Receita",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_SaidaProdutoEstoque_CompetenciaId",
                table: "SaidaProdutoEstoque",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_SaidaProdutoEstoque_EstoqueId",
                table: "SaidaProdutoEstoque",
                column: "EstoqueId");

            migrationBuilder.CreateIndex(
                name: "IX_SaidaProdutoEstoque_ProdutoId",
                table: "SaidaProdutoEstoque",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tela_TelaSistemaFilhaId",
                table: "Tela",
                column: "TelaSistemaFilhaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_CpfCnpj",
                table: "Usuario",
                column: "CpfCnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Login",
                table: "Usuario",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ValorProduto_ProdutoId",
                table: "ValorProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_ValorProduto_TipoValorProdutoId",
                table: "ValorProduto",
                column: "TipoValorProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_ClienteId",
                table: "Venda",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_CompetenciaId",
                table: "Venda",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_EmpresaId",
                table: "Venda",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_UsuarioId",
                table: "Venda",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_VendaProduto_CompetenciaId",
                table: "VendaProduto",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_VendaProduto_ProdutoId",
                table: "VendaProduto",
                column: "ProdutoId");

            migrationBuilder.CreateIndex(
                name: "IX_VendaProduto_VendaId",
                table: "VendaProduto",
                column: "VendaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorizacaoApi");

            migrationBuilder.DropTable(
                name: "DevolucaoCompra");

            migrationBuilder.DropTable(
                name: "DevolucaoVenda");

            migrationBuilder.DropTable(
                name: "Divida");

            migrationBuilder.DropTable(
                name: "EnderecoCliente");

            migrationBuilder.DropTable(
                name: "EnderecoEmpresa");

            migrationBuilder.DropTable(
                name: "EnderecoFornecedor");

            migrationBuilder.DropTable(
                name: "EnderecoUsuario");

            migrationBuilder.DropTable(
                name: "EntradaProdutoEstoqueDocumento");

            migrationBuilder.DropTable(
                name: "EntradaProdutoEstoqueFornecedor");

            migrationBuilder.DropTable(
                name: "EstoqueProduto");

            migrationBuilder.DropTable(
                name: "FluxoCaixaEstoque");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixaDespesa");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixaReceita");

            migrationBuilder.DropTable(
                name: "PagamentoFormaPagamento");

            migrationBuilder.DropTable(
                name: "PagamentoVendaFormaPagamento");

            migrationBuilder.DropTable(
                name: "Parametro");

            migrationBuilder.DropTable(
                name: "PermissaoUsuario");

            migrationBuilder.DropTable(
                name: "SaidaProdutoEstoque");

            migrationBuilder.DropTable(
                name: "ValorProduto");

            migrationBuilder.DropTable(
                name: "CompraProduto");

            migrationBuilder.DropTable(
                name: "VendaProduto");

            migrationBuilder.DropTable(
                name: "Endereco");

            migrationBuilder.DropTable(
                name: "Documento");

            migrationBuilder.DropTable(
                name: "EntradaProdutoEstoque");

            migrationBuilder.DropTable(
                name: "Despesa");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixa");

            migrationBuilder.DropTable(
                name: "Receita");

            migrationBuilder.DropTable(
                name: "Pagamento");

            migrationBuilder.DropTable(
                name: "FormaPagamento");

            migrationBuilder.DropTable(
                name: "Permissao");

            migrationBuilder.DropTable(
                name: "Tela");

            migrationBuilder.DropTable(
                name: "TipoValorProduto");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "TipoDocumento");

            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "PagamentoVenda");

            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropTable(
                name: "Sequence_Codigo_Produto");

            migrationBuilder.DropTable(
                name: "TipoProduto");

            migrationBuilder.DropTable(
                name: "Venda");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Competencia");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
