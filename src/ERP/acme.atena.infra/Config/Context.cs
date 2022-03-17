using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using acme.atena.infra.Map.Account;
using acme.atena.infra.Map.Security;
using acme.atena.infra.Map.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acme.atena.infra.Config
{
    public class Context : DbContext
    {
        //public Context()
        //{
        //}

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<Divida> Dividas { get; set; }
        public DbSet<FluxoDeCaixa> FluxoDeCaixas { get; set; }
        public DbSet<FluxoDeCaixaDespesa> FluxoDeCaixaDespesas { get; set; }
        public DbSet<FluxoDeCaixaReceita> FluxoDeCaixaReceitas { get; set; }
        public DbSet<Pagamento> Pagamento { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<FormaPagamento> FormaPagamentos { get; set; }
        public DbSet<PagamentoFormaPagamento> PagamentoFormaPagamentos { get; set; }
        public DbSet<PagamentoVenda> PagamentoVendas { get; set; }
        public DbSet<PagamentoVendaFormaPagamento> PagamentoVendaFormaPagamentos { get; set; }
        

        public DbSet<EntradaProdutoEstoque> EntradaProdutoEstoques { get; set; }
        public DbSet<Estoque> Estoques { get; set; }
        public DbSet<SaidaProdutoEstoque> SaidaProdutoEstoques { get; set; }
        public DbSet<EstoqueProduto> EstoqueProdutos { get; set; }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Empresa> Empresas{ get; set; }

        public DbSet<TipoValorProduto> TipoValorProdutos { get; set; }
        public DbSet<ValorProduto> ValorProduto { get; set; }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraProduto> CompraProdutos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<TipoProduto> TipoProduto{ get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<VendaProduto> VendasProdutos { get; set; }
        public DbSet<DevolucaoCompra> DevolucaoCompras { get; set; }
        public DbSet<DevolucaoVenda> DevolucaoVendas { get; set; }

        public DbSet<AutorizacaoApi> AutorizacaoApis { get; set; }
        public DbSet<Permissao> Permissaos { get; set; }
        public DbSet<PermissaoUsuario> PermissaoUsuarios { get; set; }
        public DbSet<Tela> Telas{ get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        
        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<EnderecoCliente> EnderecoCliente { get; set; }
        public DbSet<EnderecoFornecedor> EnderecoFornecedor{ get; set; }
        public DbSet<EnderecoUsuario> EnderecoUsuario { get; set; }
        public DbSet<EnderecoEmpresa> EnderecoEmpresa { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<AbstractEntity>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            var stringProperties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(_ => _.GetProperties())
                .Where(_ => _.ClrType == typeof(string) && _.GetColumnType() == null);

            foreach (var item in stringProperties)
            {
                item.SetIsUnicode(false);
                if (item.GetMaxLength() == null)
                    item.SetMaxLength(255);
            }

            var decimalProperties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(_ => _.GetProperties())
                .Where(_ => (_.ClrType == typeof(decimal) || _.ClrType == typeof(decimal?)) && _.GetColumnType() == null);

            foreach (var item in decimalProperties)
            {
                if (item.GetPrecision() == null && item.GetScale() == null)
                {
                    item.SetPrecision(24);
                    item.SetScale(4);
                }
            }
         


        }
    }
}
