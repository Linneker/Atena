using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Security
{
    public class PermissaoUsuarioRepository : RepositoryBase<PermissaoUsuario>, IPermissaoUsuarioRepository
    {
        public PermissaoUsuarioRepository(Context db) : base(db)
        {
        }
    }
}
