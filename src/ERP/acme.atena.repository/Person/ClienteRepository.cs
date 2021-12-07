using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Person
{
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
        public ClienteRepository(Context db) : base(db)
        {
        }
    }
}
