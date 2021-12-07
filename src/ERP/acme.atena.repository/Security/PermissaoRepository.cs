using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Security
{
    public class PermissaoRepository : RepositoryBase<Permissao>, IPermissaoRepository
    {
        public PermissaoRepository(Context db) : base(db)
        {
        }
    }
}
