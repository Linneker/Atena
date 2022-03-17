using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Security
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
