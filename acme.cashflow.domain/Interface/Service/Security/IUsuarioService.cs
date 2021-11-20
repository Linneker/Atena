using acme.cashflow.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Interface.Service.Security
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario Login(string login, string senha);
    }
}
