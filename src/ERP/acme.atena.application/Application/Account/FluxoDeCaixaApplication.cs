
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Account
{
    public class FluxoDeCaixaApplication : ServiceBase<FluxoDeCaixa>, IFluxoDeCaixaService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IDespesaService _despesaService;
        private readonly IReceitaService _receitaService;

        public FluxoDeCaixaApplication(IFluxoDeCaixaRepository fluxoDeCaixaService):base(fluxoDeCaixaService)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaService;
        }

        public void FechamentoDeCaixa()
        {
            _fluxoDeCaixaRepository.GetAll();
        }
    }
}
