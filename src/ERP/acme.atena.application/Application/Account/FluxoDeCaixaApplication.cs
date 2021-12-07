using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Account
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
