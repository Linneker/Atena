using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Repository.Account;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Account
{
    public class FluxoDeCaixaRepository : RepositoryBase<FluxoDeCaixa>, IFluxoDeCaixaRepository
    {
        public FluxoDeCaixaRepository(Context db) : base(db)
        {
        }
    }
}
