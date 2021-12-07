using acme.atena.application.Interface.Person;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Person
{
    public class ClienteApplication: ApplicationBase<Cliente>, IClienteApplication
    {
        private readonly IClienteService _pessoaService;

        public ClienteApplication(IClienteService pessoaService):base(pessoaService)
        {
            _pessoaService = pessoaService;
        }
    }
}
