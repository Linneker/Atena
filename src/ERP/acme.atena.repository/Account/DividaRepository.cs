using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Account
{
    public class DividaRepository : RepositoryBase<Divida>, IDividaRepository
    {
        public DividaRepository(Context db) : base(db)
        {
        }
    }
}
