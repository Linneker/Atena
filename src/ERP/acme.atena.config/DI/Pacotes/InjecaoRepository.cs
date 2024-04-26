using acme.atena.domain.DTO;
using acme.atena.domain.Interface.Repository;
using acme.atena.repository;
using Microsoft.Extensions.DependencyInjection;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Repository.Product.Price;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Repository.UnitOfWork;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.repository.Account;
using acme.atena.repository.Inventory;
using acme.atena.repository.Person;
using acme.atena.repository.Product;
using acme.atena.repository.Product.Price;
using acme.atena.repository.Security;
using acme.atena.repository.UnitOfWork;
using acme.atena.repository.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acme.atena.application.Handler.Enderecos.AutoMapper;

namespace acme.atena.config.DI.Pacotes
{
    internal static class InjecaoRepository
    {
        public static void RepositoryDI(this IServiceCollection services)
        {
            //REPOSITORY
            services.AddScoped<IRepositoryBase<AbstractEntity>, RepositoryBase<AbstractEntity>>();
            services.AddScoped<IDespesaRepository, DespesaRepository>();
            services.AddScoped<IReceitaRepository, ReceitaRepository>();
            services.AddScoped<IFluxoDeCaixaRepository, FluxoDeCaixaRepository>();
            services.AddScoped<IDividaRepository, DividaRepository>();
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();

            services.AddScoped<IEstoqueRepository, EstoqueRepository>();
            services.AddScoped<IEstoqueProdutoRepository, EstoqueProdutoRepository>();
            services.AddScoped<IEntradaProdutoEstoqueRepository, EntradaProdutoEstoqueRepository>();
            services.AddScoped<ISaidaProdutoEstoqueRepository, SaidaProdutoEstoqueRepository>();

            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<ICompraProdutoRepository, CompraProdutoRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IVendaRepository, VendaRepository>();
            services.AddScoped<IVendaProdutoRepository, VendaProdutoRepository>();
            services.AddScoped<IDevolucaoCompraRepository, DevolucaoCompraRepository>();
            services.AddScoped<IDevolucaoVendaRepository, DevolucaoVendaRepository>();
            services.AddScoped<IPagamentoVendaRepository, PagamentoVendaRepository>();
            services.AddScoped<ITipoProdutoRepository, TipoProdutoRepository>();
            services.AddScoped<ITipoValorProdutoRepository, TipoValorProdutoRepository>();
            services.AddScoped<IValorProdutoRepository, ValorProdutoRepository>();

            services.AddScoped<IEmpresaRepository, EmpresaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            services.AddScoped<ICompetenciaRepository, CompetenciaRepository>();
            services.AddScoped<IParametroRepository, ParametroRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();

            services.AddScoped<IAutorizacaoApiRepository, AutorizacaoApiRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPermissaoRepository, PermissaoRepository>();
            services.AddScoped<IPermissaoUsuarioRepository, PermissaoUsuarioRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            
        }
    }
}
