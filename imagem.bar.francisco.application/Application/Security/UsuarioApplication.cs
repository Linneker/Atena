using imagem.bar.francisco.application.Interface.Security;
using imagem.bar.francisco.domain.DTO.Seguranca;
using imagem.bar.francisco.domain.Interface.Service.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.application.Application.Security
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
