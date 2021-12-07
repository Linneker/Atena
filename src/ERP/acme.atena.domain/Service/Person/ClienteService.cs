using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Person
{
    public class ClienteService : ServiceBase<Cliente>, IClienteService
    {
        private readonly IClienteRepository _pessoaRepository;
        public ClienteService(IClienteRepository repositoryBase) : base(repositoryBase)
        {
            _pessoaRepository = repositoryBase;
        }
    }
}
