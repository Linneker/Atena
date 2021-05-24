using AutoMapper;
using imagem.bar.francisco.api.ViewModel;
using imagem.bar.francisco.api.ViewModel.Account;
using imagem.bar.francisco.api.ViewModel.Security;
using imagem.bar.francisco.api.ViewModel.Util;
using imagem.bar.francisco.domain.DTO;
using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.DTO.Seguranca;
using imagem.bar.francisco.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imagem.bar.francisco.api.AutoMapper
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

        }
    }
}
