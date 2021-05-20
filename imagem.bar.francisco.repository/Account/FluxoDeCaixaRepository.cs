using imagem.bar.francisco.domain.DTO.Account;
using imagem.bar.francisco.domain.Interface.Repository.Account;
using imagem.bar.francisco.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.repository.Account
{
    public class FluxoDeCaixaRepository : RepositoryBase<FluxoDeCaixa>, IFluxoDeCaixaRepository
    {
        public FluxoDeCaixaRepository(Context db) : base(db)
        {
        }
    }
}
