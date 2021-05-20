using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Repository.Account;
using imagem.bar.francisco.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.Service.Account
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
