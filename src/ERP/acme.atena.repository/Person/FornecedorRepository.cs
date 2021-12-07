using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Person
{
    public class FornecedorRepository : RepositoryBase<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(Context db) : base(db)
        {
        }
    }
}
