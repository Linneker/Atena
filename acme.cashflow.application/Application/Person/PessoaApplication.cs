using acme.cashflow.application.Interface.Person;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Person
{
    public class PessoaApplication: ApplicationBase<Pessoa>,IPessoaApplication
    {
        private readonly IPessoaService _pessoaService;

        public PessoaApplication(IPessoaService pessoaService):base(pessoaService)
        {
            _pessoaService = pessoaService;
        }
    }
}
