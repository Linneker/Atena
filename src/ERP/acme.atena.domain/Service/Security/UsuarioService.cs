using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Security
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository): base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        
        public Usuario Login(string login, string senha)
        {

            return _usuarioRepository.Login(new Usuario(login,senha));
        }
    }
}
