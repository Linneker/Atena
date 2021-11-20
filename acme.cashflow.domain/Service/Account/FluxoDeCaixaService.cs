using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Repository.Account;
using acme.cashflow.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Account
{
    public class FluxoDeCaixaService : ServiceBase<FluxoDeCaixa>, IFluxoDeCaixaService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;

        public FluxoDeCaixaService(IFluxoDeCaixaRepository fluxoDeCaixaRepository):base(fluxoDeCaixaRepository)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
        }
    }
}
