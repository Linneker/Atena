using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Security
{
    public class PermissaoUsuarioService: ServiceBase<PermissaoUsuario>, IPermissaoUsuarioService
    {
        private readonly IPermissaoUsuarioRepository _permissaoUsuarioRepository;

        public PermissaoUsuarioService(IPermissaoUsuarioRepository permissaoUsuarioRepository):base(permissaoUsuarioRepository)
        {
            _permissaoUsuarioRepository = permissaoUsuarioRepository;
        }
    }
}
