using acme.atena.application.Service;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Person
{
    public class ClienteService: ServiceBase<Cliente>, IClienteService
    {
        private readonly IClienteRepository _pessoaService;

        public ClienteService(IClienteRepository pessoaService):base(pessoaService)
        {
            _pessoaService = pessoaService;
        }
    }
}
