using AutoMapper;
using acme.atena.api.ViewModel;
using acme.atena.api.ViewModel.Account;
using acme.atena.api.ViewModel.Security;
using acme.atena.api.ViewModel.Util;
using acme.atena.domain.DTO;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acme.atena.api.ViewModel.Person;
using acme.atena.domain.DTO.Person;
using acme.atena.api.ViewModel.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.api.ViewModel.Product.Price;
using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.DTO.Inventory;
using acme.atena.api.ViewModel.Inventory;

namespace acme.atena.api.AutoMapper
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration() : this("MyProfile")
        {
        }
        protected AutoMapperProfileConfiguration(string profileName) : base(profileName)
        {
            CreateMap<AbstractEntityViewModel, AbstractEntity>();
            CreateMap<AbstractEntity, AbstractEntityViewModel>();

            CreateMap<NotificationBaseViewModel, NotificationBase>();
            CreateMap<NotificationBase, NotificationBaseViewModel>();

            CreateMap<ResponseApiViewModel, ResponseApi>();
            CreateMap<ResponseApi, ResponseApiViewModel>();

            CreateMap<NotificationViewModel, Notification>();
            CreateMap<Notification, NotificationViewModel>();

            CreateMap<CompetenciaViewModel, Competencia>();
            CreateMap<Competencia, CompetenciaViewModel>();

            CreateMap<ReceitaViewModel, Receita>();
            CreateMap<Receita, ReceitaViewModel>();

            CreateMap<DespesaViewModel, Despesa>();
            CreateMap<Despesa, DespesaViewModel>();

            CreateMap<FluxoDeCaixaViewModel, FluxoDeCaixa>();
            CreateMap<FluxoDeCaixa, FluxoDeCaixaViewModel>();

            CreateMap<FluxoDeCaixaDespesaViewModel, FluxoDeCaixaDespesa>();
            CreateMap<FluxoDeCaixaDespesa, FluxoDeCaixaDespesaViewModel>();

            CreateMap<FluxoDeCaixaReceitaViewModel, FluxoDeCaixaReceita>();
            CreateMap<FluxoDeCaixaReceita, FluxoDeCaixaReceitaViewModel>();

            CreateMap<AutorizacaoApiViewModel, AutorizacaoApi>();
            CreateMap<AutorizacaoApi, AutorizacaoApiViewModel>();

            CreateMap<Usuario, UsuarioViewModel>();
            CreateMap<UsuarioViewModel,Usuario>();
            
            CreateMap<Permissao, PermissaoViewModel>();
            CreateMap<PermissaoViewModel, Permissao>();
            
            CreateMap<PermissaoUsuario, PermissaoUsuarioViewModel>();
            CreateMap<PermissaoUsuarioViewModel, PermissaoUsuario>();

            CreateMap<Divida, DividaViewModel>();
            CreateMap<DividaViewModel, Divida>();
            CreateMap<Pagamento, PagamentoViewModel>();
            CreateMap<PagamentoViewModel, Pagamento>();


            CreateMap<Cliente, ClienteViewModel>();
            CreateMap<ClienteViewModel, Cliente>();
            CreateMap<Fornecedor, FornecedorViewModel>();
            CreateMap<FornecedorViewModel, Fornecedor>();

            CreateMap<Compra, CompraViewModel>();
            CreateMap<CompraViewModel, Compra>();
            
            CreateMap<CompraProduto, CompraProdutoViewModel>();
            CreateMap<CompraProdutoViewModel, CompraProduto>();
            
            CreateMap<Produto, ProdutoViewModel>();
            CreateMap<ProdutoViewModel, Produto>();

            CreateMap<TipoProduto, TipoProdutoViewModel>();
            CreateMap<TipoProdutoViewModel, TipoProduto>();

            CreateMap<Venda, VendaViewModel>();
            CreateMap<VendaViewModel, Venda>();
            
            CreateMap<VendaProduto, VendaProdutoViewModel>();
            CreateMap<VendaProdutoViewModel, VendaProduto>();

            CreateMap<Parametro, ParametroViewModel>();
            CreateMap<ParametroViewModel, Parametro>();

            CreateMap<Empresa, EmpresaViewModel>();
            CreateMap<EmpresaViewModel, Empresa>();

            CreateMap<Endereco, EnderecoViewModel>();
            CreateMap<EnderecoViewModel, Endereco>();

            CreateMap<EnderecoEmpresa, EnderecoEmpresaViewModel>();
            CreateMap<EnderecoEmpresaViewModel, EnderecoEmpresa>();

            CreateMap<EnderecoUsuario, EnderecoUsuarioViewModel>();
            CreateMap<EnderecoUsuarioViewModel, EnderecoUsuario>();

            CreateMap<EnderecoFornecedor, EnderecoFornecedorViewModel>();
            CreateMap<EnderecoFornecedorViewModel, EnderecoFornecedor>();

            CreateMap<TipoValorProduto, TipoValorProdutoViewModel>();
            CreateMap<TipoValorProdutoViewModel, TipoValorProduto>();

            CreateMap<ValorProduto, ValorProdutoViewModel>();
            CreateMap<ValorProdutoViewModel, ValorProduto>();

            CreateMap<Estoque, EstoqueViewModel>();
            CreateMap<EstoqueViewModel, Estoque>();

            CreateMap<EntradaProdutoEstoque, EntradaProdutoEstoqueViewModel>();
            CreateMap<EntradaProdutoEstoqueViewModel, EntradaProdutoEstoque>();

            CreateMap<SaidaProdutoEstoque, SaidaProdutoEstoqueViewModel>();
            CreateMap<SaidaProdutoEstoqueViewModel, SaidaProdutoEstoque>();

            CreateMap<EstoqueProduto, EstoqueProdutoViewModel>();
            CreateMap<EstoqueProdutoViewModel, EstoqueProduto>();

            
        }
    }
}
