using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Repository.Security
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario> Login(Usuario usuario);
        Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync();
    }
}
