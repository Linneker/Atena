using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Interface.Service.Security
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario Login(string login, string senha);
    }
}
