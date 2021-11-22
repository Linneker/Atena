using acme.cashflow.domain.DTO;
using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using acme.cashflow.infra.Map.Account;
using acme.cashflow.infra.Map.Security;
using acme.cashflow.infra.Map.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acme.cashflow.infra.Config
{
    public class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<FluxoDeCaixa> FluxoDeCaixas { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<Despesa> Despesas { get; set; }
        public DbSet<FluxoDeCaixaDespesa> FluxoDeCaixaDespesas { get; set; }
        public DbSet<FluxoDeCaixaReceita> FluxoDeCaixaReceitas { get; set; }
        public DbSet<Divida> Dividas { get; set; }
        public DbSet<Pagamento> Pagamento { get; set; }

        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }

        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraProduto> CompraProdutos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Venda> Vendas { get; set; }
        public DbSet<VendaProduto> VendasProdutos { get; set; }

        public DbSet<AutorizacaoApi> AutorizacaoApis { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Permissao> Permissaos { get; set; }
        public DbSet<PermissaoUsuario> PermissaoUsuarios { get; set; }

        public DbSet<Competencia> Competencias { get; set; }
        public DbSet<Parametro> Parametros { get; set; }

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
                    item.SetMaxLength(256);
            }

            var decimalProperties = modelBuilder.Model.GetEntityTypes()
                .SelectMany(_ => _.GetProperties())
                .Where(_ => (_.ClrType == typeof(decimal) || _.ClrType == typeof(decimal?)) && _.GetColumnType() == null);

            foreach (var item in decimalProperties)
            {
                if (item.GetPrecision() == null && item.GetScale() == null)
                {
                    item.SetPrecision(18);
                    item.SetScale(4);
                }
            }
            modelBuilder.ApplyConfiguration(new CompetenciaMap());

            modelBuilder.ApplyConfiguration(new DespesaMap());
            modelBuilder.ApplyConfiguration(new ReceitaMap());
            modelBuilder.ApplyConfiguration(new FluxoDeCaixaMap());
            modelBuilder.ApplyConfiguration(new FluxoDeCaixaReceitaMap());
            modelBuilder.ApplyConfiguration(new FluxoDeCaixaDespesaMap());

            modelBuilder.ApplyConfiguration(new AutorizacaoApiMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());


        }
    }
}
