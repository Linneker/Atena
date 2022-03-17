using System;
using acme.atena.domain.DTO.Seguranca;
using Microsoft.EntityFrameworkCore.Migrations;

namespace acme.atena.infra.Migrations
{
    public partial class PrimeiroMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutorizacaoApi",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    AccessKey = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacaoApi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    NomeFantasia = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.Id);
                });

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
                name: "Empresa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    IsFilial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EmpresaMatrizId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    RazaoSocial = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Endereco",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Cep = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Pais = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Estado = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Cidade = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Bairro = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Rua = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Endereco", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fornecedor",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    NomeFantasia = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fornecedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parametro",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Descricao = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    Valor = table.Column<string>(type: "varchar(1500)", unicode: false, maxLength: 1500, nullable: true),
                    ExibirTela = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parametro", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissao",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(110)", unicode: false, maxLength: 110, nullable: true),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoProduto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProduto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoValorProduto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoValorProduto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Login = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Senha = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true),
                    TermoDeAceite = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Nome = table.Column<string>(type: "varchar(400)", unicode: false, maxLength: 400, nullable: true),
                    CpfCnpj = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    Celular = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Despesa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FluxoDeCaixa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    SaldoOperacional = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    SaldoFinalCaixa = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receita",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Descricao = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: false),
                    IsPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    EstoqueMaximo = table.Column<int>(type: "int", nullable: false),
                    EstoqueMinimo = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "EnderecoCliente",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ClienteId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    EnderecoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "EnderecoEmpresa",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EmpresaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    EnderecoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "EnderecoFornecedor",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    FornecedorId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    EnderecoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Nome = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Descricao = table.Column<string>(type: "varchar(2000)", unicode: false, maxLength: 2000, nullable: true),
                    Codigo = table.Column<long>(type: "bigint", nullable: false),
                    Validade = table.Column<DateTime>(type: "datetime", nullable: true),
                    TipoProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produto_TipoProduto_TipoProdutoId",
                        column: x => x.TipoProdutoId,
                        principalTable: "TipoProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Compra",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime", nullable: false),
                    ValorTotal = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    FornecedorId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Divida",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EnumTipoDivida = table.Column<int>(type: "int", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    DataQueDeviaTerRecebido = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataCompra = table.Column<DateTime>(type: "datetime", nullable: false),
                    UsuarioId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    FornecedorId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    ClienteId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EmpresaId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "EnderecoUsuario",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    UsuarioId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Numero = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Complemento = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Latitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Longitude = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    EnderecoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "Pagamento",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EnumFormaPagamento = table.Column<int>(type: "int", nullable: false),
                    Parcelado = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    NumeroDeParcela = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DataPagamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    DataCredito = table.Column<DateTime>(type: "datetime", nullable: true),
                    ValorPago = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    EnumTipoPagamento = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    FornecedorId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    ClienteId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EmpresaId = table.Column<byte[]>(type: "varbinary(16)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
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
                        name: "FK_Pagamento_Usuario_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissaoUsuario",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Read = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Add = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Delete = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    Update = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    UsuarioId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    PermissaoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
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
                        name: "FK_PermissaoUsuario_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Venda",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataVenda = table.Column<DateTime>(type: "datetime", nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    UsuarioId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    PessoaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Venda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Venda_Cliente_PessoaId",
                        column: x => x.PessoaId,
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
                        name: "FK_Venda_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaDespesa_FluxoDeCaixa_FluxoDeCaixaId",
                        column: x => x.FluxoDeCaixaId,
                        principalTable: "FluxoDeCaixa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FluxoDeCaixaReceita_Receita_ReceitaId",
                        column: x => x.ReceitaId,
                        principalTable: "Receita",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntradaProdutoEstoque",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EstoqueId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataEntrada = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "NOW()"),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "EstoqueProduto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EstoqueId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    QuantidadeMaxima = table.Column<long>(type: "bigint", nullable: false),
                    QuantidadeMinima = table.Column<long>(type: "bigint", nullable: false),
                    QuantidadeProduto = table.Column<long>(type: "bigint", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "SaidaProdutoEstoque",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    EstoqueId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    DataSaida = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "NOW()"),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "ValorProduto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    TipoValorProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
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
                        name: "FK_ValorProduto_TipoValorProduto_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "TipoValorProduto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompraProduto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    QuantidadeComprada = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsRecebido = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    CompraId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "VendaProduto",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18, 2)", precision: 24, scale: 4, nullable: false),
                    QuantidadeVedida = table.Column<int>(type: "int", nullable: false),
                    DataPagamento = table.Column<DateTime>(type: "datetime", nullable: false),
                    Enviado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VendaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                        name: "FK_VendaProduto_Venda_ProdutoId",
                        column: x => x.ProdutoId,
                        principalTable: "Venda",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DevolucaoCompra",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    CompraProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    IsParcial = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "DevolucaoVenda",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Motivo = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    VendaProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    ProdutoId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    IsParcial = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataModificacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    DataInativacao = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CompetenciaId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
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
                });

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
                name: "IX_Pagamento_UsuarioId",
                table: "Pagamento",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoUsuario_PermissaoId",
                table: "PermissaoUsuario",
                column: "PermissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissaoUsuario_UsuarioId",
                table: "PermissaoUsuario",
                column: "UsuarioId");

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
                name: "IX_Venda_CompetenciaId",
                table: "Venda",
                column: "CompetenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Venda_PessoaId",
                table: "Venda",
                column: "PessoaId");

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

            migrationBuilder.InsertData("AutorizacaoApi",
             new string[] { "Id", "AccessKey", "DataCriacao", "Status" },
             new object[] { "UUID_TO_BIN(UUID())", "1c4fa75b2ca64019a42a3a74c7147171", DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
          new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
          new object[] { "UUID_TO_BIN('E7DE8C01-5A2A-40E0-AEEF-AB72C10DB880')", "Root", 0, DateTime.Now, 0 });


            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Adminsitrador", 1, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Geral", 2, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "TI", 2, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente RH", 3, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Fiscal", 3, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Administracao", 3, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Contabil", 3, DateTime.Now, 0 });
            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Venda", 3, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Compra", 3, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Gerente Logistica", 3, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
              new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
              new object[] { "UUID_TO_BIN(UUID())", "Gerente_RH", 4, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Fiscal", 4, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Administracao", 4, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Contabil", 4, DateTime.Now, 0 });
            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Venda", 4, DateTime.Now, 0 });
            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Compra", 4, DateTime.Now, 0 });

            migrationBuilder.InsertData("Permissao",
               new string[] { "Id", "Nome", "Nivel", "DataCriacao", "Status" },
               new object[] { "UUID_TO_BIN(UUID())", "Logistica", 4, DateTime.Now, 0 });

            migrationBuilder.InsertData("Usuario",
             new string[] { "Id", "Nome", "CpfCnpj", "Email", "Celular", "DataNascimento", "Login", "Senha", "TermoDeAceite", "DataCriacao", "Status" },
             new object[] { "UUID_TO_BIN('0CE57EBC-D000-4DCD-B7CA-3A868DF0E673')", "Root", "12486468609", "linneker.blytner@gmail.com", "(35)99244-5418", "1994-05-05", "root", Usuario.SHA512(Usuario.SHA512(Usuario.SHA512("0CE57EBCD0004DCDB7CA3A868DF0E673"))), true, DateTime.Now, 0 });

            migrationBuilder.InsertData("PermissaoUsuario",
             new string[] { "Id", "Read", "Add", "Delete", "Update", "UsuarioId", "PermissaoId", "DataCriacao", "Status" },
             new object[] { "UUID_TO_BIN(UUID())", true, true, true, true, "UUID_TO_BIN('0CE57EBC-D000-4DCD-B7CA-3A868DF0E673')", "UUID_TO_BIN('E7DE8C01-5A2A-40E0-AEEF-AB72C10DB880')", DateTime.Now, 0 });

            migrationBuilder.InsertData("Competencia",
            new string[] { "Id", "Ano", "Mes", "DataCriacao", "Status" },
             new object[] { "UUID_TO_BIN('CE872663-4879-4406-8194-2C78844EF23F')", 2021, 12, DateTime.Now, 0 });

            migrationBuilder.InsertData("FluxoDeCaixa",
           new string[] { "Id", "SaldoOperacional", "SaldoFinalCaixa","SaldoInicial", "CompetenciaId", "DataCriacao", "Status" },
           new object[] { "UUID_TO_BIN(UUID())", 0, 0, 0, "UUID_TO_BIN('CE872663-4879-4406-8194-2C78844EF23F')", DateTime.Now, 0 });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM AutorizacaoApi");
            migrationBuilder.Sql("DELETE FROM Permissao");

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
                name: "EntradaProdutoEstoque");

            migrationBuilder.DropTable(
                name: "EstoqueProduto");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixaDespesa");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixaReceita");

            migrationBuilder.DropTable(
                name: "Pagamento");

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
                name: "Despesa");

            migrationBuilder.DropTable(
                name: "FluxoDeCaixa");

            migrationBuilder.DropTable(
                name: "Receita");

            migrationBuilder.DropTable(
                name: "Permissao");

            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.DropTable(
                name: "TipoValorProduto");

            migrationBuilder.DropTable(
                name: "Compra");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Venda");

            migrationBuilder.DropTable(
                name: "Empresa");

            migrationBuilder.DropTable(
                name: "Fornecedor");

            migrationBuilder.DropTable(
                name: "TipoProduto");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Competencia");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
