using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.Interface.Repository.Person;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Person
{
    public class PessoaRepository : RepositoryBase<Pessoa>, IPessoaRepository
    {
        public PessoaRepository(Context db) : base(db)
        {
        }
    }
}
