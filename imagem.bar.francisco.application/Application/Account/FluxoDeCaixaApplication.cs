using imagem.bar.francisco.application.Interface.Util;
using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.application.Application.Account
{
    public class FluxoDeCaixaApplication : ApplicationBase<FluxoDeCaixa>, IFluxoDeCaixaApplication
    {
        private readonly IFluxoDeCaixaService _fluxoDeCaixaService;

        public FluxoDeCaixaApplication(IFluxoDeCaixaService fluxoDeCaixaService):base(fluxoDeCaixaService)
        {
            _fluxoDeCaixaService = fluxoDeCaixaService;
        }
    }
}
