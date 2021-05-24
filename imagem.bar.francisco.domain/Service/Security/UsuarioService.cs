using imagem.bar.francisco.domain.DTO.Seguranca;
using imagem.bar.francisco.domain.Interface.Repository.Security;
using imagem.bar.francisco.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.Service.Security
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
