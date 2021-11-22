using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Security
{
    public class PermissaoUsuarioRepository : RepositoryBase<PermissaoUsuario>, IPermissaoUsuarioRepository
    {
        public PermissaoUsuarioRepository(Context db) : base(db)
        {
        }
    }
}
