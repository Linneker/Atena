using acme.atena.application.Service;
using acme.atena.application.Service.Account;
using acme.atena.application.Service.Inventory;
using acme.atena.application.Service.Person;
using acme.atena.application.Service.Product;
using acme.atena.application.Service.Product.Price;
using acme.atena.application.Service.Security;
using acme.atena.application.Service.Util;
using acme.atena.domain.DTO;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Account;
using acme.atena.domain.Interface.Service.Inventory;
using acme.atena.domain.Interface.Service.Person;
using acme.atena.domain.Interface.Service.Product;
using acme.atena.domain.Interface.Service.Product.Price;
using acme.atena.domain.Interface.Service.Security;
using acme.atena.domain.Interface.Service.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.config.DI.Pacotes
{
    internal static class InjecaoService
    {
        public static void DIService(this IServiceCollection services)
        {
            //SERVICE
            services.AddTransient<IServiceBase<AbstractEntity>, ServiceBase<AbstractEntity>>();
            services.AddTransient<IDespesaService, DespesaService>();
            services.AddTransient<IReceitaService, ReceitaService>();
            services.AddTransient<IFluxoDeCaixaService, FluxoDeCaixaService>();
            services.AddTransient<IDividaService, DividaService>();
            services.AddTransient<IPagamentoService, PagamentoService>();
            services.AddTransient<IPagamentoVendaService, PagamentoVendaService>();

            services.AddTransient<IEstoqueService, EstoqueService>();
            services.AddTransient<IEstoqueProdutoService, EstoqueProdutoService>();
            services.AddTransient<IEntradaProdutoEstoqueService, EntradaProdutoEstoqueService>();
            services.AddTransient<ISaidaProdutoEstoqueService, SaidaProdutoEstoqueService>();


            services.AddTransient<ICompraService, CompraService>();
            services.AddTransient<ICompraProdutoService, CompraProdutoService>();
            services.AddTransient<IProdutoService, ProdutoService>();
            services.AddTransient<IVendaService, VendaService>();
            services.AddTransient<IVendaProdutoService, VendaProdutoService>();
            services.AddTransient<IDevolucaoCompraService, DevolucaoCompraService>();
            services.AddTransient<IDevolucaoVendaService, DevolucaoVendaService>();
            services.AddTransient<ITipoProdutoService, TipoProdutoService>();
            services.AddTransient<ITipoValorProdutoService, TipoValorProdutoService>();

            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<IFornecedorService, FornecedorService>();

            services.AddTransient<ICompetenciaService, CompetenciaService>();
            services.AddTransient<IParametroService, ParametroService>();
            services.AddTransient<IEnderecoService, EnderecoService>();

            services.AddTransient<IAutorizacaoApiService, AutorizacaoApiService>();
            services.AddTransient<IUsuarioService, UsuarioService>();
            services.AddTransient<IPermissaoService, PermissaoService>();
            services.AddTransient<IPermissaoUsuarioService, PermissaoUsuarioService>();

        }
    }
}
