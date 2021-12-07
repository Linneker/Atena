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
