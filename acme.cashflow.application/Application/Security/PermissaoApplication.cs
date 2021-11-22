using acme.cashflow.application.Interface.Security;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Security
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
