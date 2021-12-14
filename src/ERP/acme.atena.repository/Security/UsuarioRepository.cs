using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.Interface.Repository.Security;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.repository.Security
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(Context db) : base(db)
        {
        }

        public Usuario Login(Usuario usuario)
        {
            var query = (from user in _db.Usuarios
                         join userPerm in _db.PermissaoUsuarios on user.Id equals userPerm.UsuarioId
                         join perm in _db.Permissaos on userPerm.PermissaoId equals perm.Id
                         where user.Login.Equals(usuario.Login) && user.Senha.Equals(usuario.Senha)
                         select user).Include(t=>t.PermissaoUsuarios).ThenInclude(t=>t.Permissao).FirstOrDefault();
            return query;
        }

        public Task<IQueryable<Usuario>> RecuperaUsuariosComPermissaoAssync()
        {
            var query = (from user in _db.Usuarios
                         join userPerm in _db.PermissaoUsuarios on user.Id equals userPerm.UsuarioId
                         join perm in _db.Permissaos on userPerm.PermissaoId equals perm.Id
                         select user).Include(t => t.PermissaoUsuarios).ThenInclude(t => t.Permissao).AsQueryable();
            return Task.FromResult(query);
        }
    }
}
