using acme.atena.application.Interface.Security;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Security
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

        public Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync()
        {
            return _usuarioService.RecuperaUsuariosComPermissaoAssync();
        }
    }
}
