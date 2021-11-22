using AutoMapper;
using acme.cashflow.api.ViewModel;
using acme.cashflow.api.ViewModel.Account;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.api.ViewModel.Util;
using acme.cashflow.domain.DTO;
using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.api.ViewModel.Product;
using acme.cashflow.domain.DTO.Product;

namespace acme.cashflow.api.AutoMapper
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


            CreateMap<Pessoa, PessoaViewModel>();
            CreateMap<PessoaViewModel, Pessoa>();
            CreateMap<Fornecedor, FornecedorViewModel>();
            CreateMap<FornecedorViewModel, Fornecedor>();

            CreateMap<Compra, CompraViewModel>();
            CreateMap<CompraViewModel, Compra>();
            
            CreateMap<CompraProduto, CompraProdutoViewModel>();
            CreateMap<CompraProdutoViewModel, CompraProduto>();
            
            CreateMap<Produto, ProdutoViewModel>();
            CreateMap<ProdutoViewModel, Produto>();
            
            CreateMap<Venda, VendaViewModel>();
            CreateMap<VendaViewModel, Venda>();
            
            CreateMap<VendaProduto, VendaProdutoViewModel>();
            CreateMap<VendaProdutoViewModel, VendaProduto>();

            CreateMap<Parametro, ParametroViewModel>();
            CreateMap<ParametroViewModel, Parametro>();

        }
    }
}
