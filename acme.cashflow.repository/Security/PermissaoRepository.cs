using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Security
{
    public class PermissaoRepository : RepositoryBase<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(Context db) : base(db)
        {
        }
    }
}
