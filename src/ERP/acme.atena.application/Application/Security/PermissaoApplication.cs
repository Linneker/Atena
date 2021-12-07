using acme.atena.application.Interface.Security;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Security
{
    public class PermissaoApplication: ApplicationBase<Permissao>, IPermissaoApplication
    {
        private readonly IPermissaoService _permissaoService;

        public PermissaoApplication(IPermissaoService permissaoService):base(permissaoService)
        {
            _permissaoService = permissaoService;
        }
    }
}
