using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Repository.Util;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Util
{
    public class ParametroRepository : RepositoryBase<Parametro>, IParametroRepository
    {
        public ParametroRepository(Context db) : base(db)
        {
        }
    }
}
