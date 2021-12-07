using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Account
{
    public class FluxoDeCaixaService : ServiceBase<FluxoDeCaixa>, IFluxoDeCaixaService
    {
        private readonly IFluxoDeCaixaRepository _fluxoDeCaixaRepository;
        private readonly IDespesaService _despesaService;
        private readonly IReceitaService _receitaService;

        public FluxoDeCaixaService(IFluxoDeCaixaRepository fluxoDeCaixaRepository,
            IDespesaService despesaService,
            IReceitaService receitaService) :base(fluxoDeCaixaRepository)
        {
            _fluxoDeCaixaRepository = fluxoDeCaixaRepository;
            _despesaService = despesaService;
            _receitaService = receitaService;
        }
        public void FechamentoDeCaixa()
        {
            _fluxoDeCaixaRepository.GetAll();
        }
    }
}
