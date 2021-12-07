using acme.atena.application.Interface.Security;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Security
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
