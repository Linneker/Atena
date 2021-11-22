using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.Interface.Repository.Person;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Person
{
    public class FornecedorRepository : RepositoryBase<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(Context db) : base(db)
        {
        }
    }
}
