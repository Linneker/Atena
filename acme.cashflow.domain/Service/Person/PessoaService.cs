using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Person;
using acme.cashflow.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Person
{
    public class PessoaService : ServiceBase<Pessoa>, IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        public PessoaService(IPessoaRepository repositoryBase) : base(repositoryBase)
        {
            _pessoaRepository = repositoryBase;
        }
    }
}
