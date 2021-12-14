using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Security
{
    public interface IUsuarioApplication: IApplicationBase<Usuario>
    {
        Usuario Login(string usuario, string senha);
        Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync();

    }
}
