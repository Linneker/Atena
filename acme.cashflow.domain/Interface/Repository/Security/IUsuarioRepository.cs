using acme.cashflow.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Interface.Repository.Security
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Usuario Login(Usuario usuario);
    }
}
