using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Security
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        Usuario Login(string login, string senha);
        Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync();

    }
}
