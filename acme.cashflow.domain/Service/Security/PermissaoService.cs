using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Security
{
    public class PermissaoService : ServiceBase<Permissao>, IPermissaoService
    {
        private readonly IPermissaoRepository _permissaoRepository;

        public PermissaoService(IPermissaoRepository permissaoRepository) : base(permissaoRepository)
        {
            _permissaoRepository = permissaoRepository;
        }
    }
}
