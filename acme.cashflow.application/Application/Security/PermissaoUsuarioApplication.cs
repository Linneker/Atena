using acme.cashflow.application.Interface.Security;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Security
{
    public class PermissaoUsuarioApplication: ApplicationBase<PermissaoUsuario>, IPermissaoUsuarioApplication
    {
        private readonly IPermissaoUsuarioService _permissaoUsuarioService;

        public PermissaoUsuarioApplication(IPermissaoUsuarioService permissaoUsuarioService):base(permissaoUsuarioService)
        {
            _permissaoUsuarioService = permissaoUsuarioService;
        }
    }
}
