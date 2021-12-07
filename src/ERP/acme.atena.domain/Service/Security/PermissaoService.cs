using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Security
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
