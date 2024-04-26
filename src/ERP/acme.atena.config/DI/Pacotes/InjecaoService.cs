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
        public static void ServiceDI(this IServiceCollection services)
        {
            //SERVICE
            services.AddScoped<IServiceBase<AbstractEntity>, ServiceBase<AbstractEntity>>();
            services.AddScoped<IDespesaService, DespesaService>();
            services.AddScoped<IReceitaService, ReceitaService>();
            services.AddScoped<IFluxoDeCaixaService, FluxoDeCaixaService>();
            services.AddScoped<IDividaService, DividaService>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoVendaService, PagamentoVendaService>();

            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<IEstoqueProdutoService, EstoqueProdutoService>();
            services.AddScoped<IEntradaProdutoEstoqueService, EntradaProdutoEstoqueService>();
            services.AddScoped<ISaidaProdutoEstoqueService, SaidaProdutoEstoqueService>();
            services.AddScoped<IValorProdutoService, ValorProdutoService>();
            

            services.AddScoped<ICompraService, CompraService>();
            services.AddScoped<ICompraProdutoService, CompraProdutoService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<IVendaService, VendaService>();
            services.AddScoped<IVendaProdutoService, VendaProdutoService>();
            services.AddScoped<IDevolucaoCompraService, DevolucaoCompraService>();
            services.AddScoped<IDevolucaoVendaService, DevolucaoVendaService>();
            services.AddScoped<ITipoProdutoService, TipoProdutoService>();
            services.AddScoped<ITipoValorProdutoService, TipoValorProdutoService>();

            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IFornecedorService, FornecedorService>();

            services.AddScoped<ICompetenciaService, CompetenciaService>();
            services.AddScoped<IParametroService, ParametroService>();
            services.AddScoped<IEnderecoService, EnderecoService>();

            services.AddScoped<IAutorizacaoApiService, AutorizacaoApiService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IPermissaoService, PermissaoService>();
            services.AddScoped<IPermissaoUsuarioService, PermissaoUsuarioService>();

        }
    }
}
