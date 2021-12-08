using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Account
{
    public class PagamentoVendaRepository : RepositoryBase<PagamentoVenda>, IPagamentoVendaRepository
    {
        public PagamentoVendaRepository(Context db) : base(db)
        {
        }
    }
}
