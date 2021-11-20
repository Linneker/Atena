using acme.cashflow.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Interface.Security
{
    public interface IUsuarioApplication: IApplicationBase<Usuario>
    {
        Usuario Login(string usuario, string senha);
    }
}
