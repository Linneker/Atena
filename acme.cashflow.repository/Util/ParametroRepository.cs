using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.DTO.Util;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.domain.Interface.Repository.Util;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Util
{
    public class ParametroRepository : RepositoryBase<Parametro>, IParametroRepository
    {
        public ParametroRepository(Context db) : base(db)
        {
        }
    }
}
