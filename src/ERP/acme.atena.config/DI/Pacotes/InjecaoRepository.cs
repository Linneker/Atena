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

namespace acme.atena.config.DI.Pacotes
{
    internal static class InjecaoRepository
    {
        public static void IDRepository(this IServiceCollection services)
        {
            //REPOSITORY
            services.AddTransient<IRepositoryBase<AbstractEntity>, RepositoryBase<AbstractEntity>>();
            services.AddTransient<IDespesaRepository, DespesaRepository>();
            services.AddTransient<IReceitaRepository, ReceitaRepository>();
            services.AddTransient<IFluxoDeCaixaRepository, FluxoDeCaixaRepository>();
            services.AddTransient<IDividaRepository, DividaRepository>();
            services.AddTransient<IPagamentoRepository, PagamentoRepository>();

            services.AddTransient<IEstoqueRepository, EstoqueRepository>();
            services.AddTransient<IEstoqueProdutoRepository, EstoqueProdutoRepository>();
            services.AddTransient<IEntradaProdutoEstoqueRepository, EntradaProdutoEstoqueRepository>();
            services.AddTransient<ISaidaProdutoEstoqueRepository, SaidaProdutoEstoqueRepository>();

            services.AddTransient<ICompraRepository, CompraRepository>();
            services.AddTransient<ICompraProdutoRepository, CompraProdutoRepository>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IVendaRepository, VendaRepository>();
            services.AddTransient<IVendaProdutoRepository, VendaProdutoRepository>();
            services.AddTransient<IDevolucaoCompraRepository, DevolucaoCompraRepository>();
            services.AddTransient<IDevolucaoVendaRepository, DevolucaoVendaRepository>();
            services.AddTransient<IPagamentoVendaRepository, PagamentoVendaRepository>();
            services.AddTransient<ITipoProdutoRepository, TipoProdutoRepository>();
            services.AddTransient<ITipoValorProdutoRepository, TipoValorProdutoRepository>();


            services.AddTransient<IEmpresaRepository, EmpresaRepository>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IFornecedorRepository, FornecedorRepository>();

            services.AddTransient<ICompetenciaRepository, CompetenciaRepository>();
            services.AddTransient<IParametroRepository, ParametroRepository>();
            services.AddTransient<IEnderecoRepository, EnderecoRepository>();

            services.AddTransient<IAutorizacaoApiRepository, AutorizacaoApiRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IPermissaoRepository, PermissaoRepository>();
            services.AddTransient<IPermissaoUsuarioRepository, PermissaoUsuarioRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

        }
    }
}
