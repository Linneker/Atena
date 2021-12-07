using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Interface.Repository.Security
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Usuario Login(Usuario usuario);
    }
}
