using acme.cashflow.application.Interface.Util;
using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Account
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
