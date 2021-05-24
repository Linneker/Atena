using imagem.bar.francisco.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace imagem.bar.francisco.domain.Interface.Service.Security
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario Login(string login, string senha);
    }
}
