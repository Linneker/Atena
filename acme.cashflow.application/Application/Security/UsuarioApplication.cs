using acme.cashflow.application.Interface.Security;
using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Security
{
    public class UsuarioApplication: ApplicationBase<Usuario>, IUsuarioApplication
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioApplication(IUsuarioService usuarioService):base(usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public Usuario Login(string usuario, string senha)
        {
            return _usuarioService.Login(usuario, senha);
        }
    }
}
