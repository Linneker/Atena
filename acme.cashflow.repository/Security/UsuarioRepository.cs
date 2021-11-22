﻿using acme.cashflow.domain.DTO.Seguranca;
using acme.cashflow.domain.Interface.Repository.Security;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace acme.cashflow.repository.Security
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(Context db) : base(db)
        {
        }

        public Usuario Login(Usuario usuario)
        {
            var query = (from user in _db.Usuarios
                         where user.Login.Equals(usuario.Login) && user.Senha.Equals(usuario.Senha)
                         select user).FirstOrDefault();
            return query;
        }
    }
}