using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Service.Person
{
    public class FornecedorService : ServiceBase<Fornecedor>, IFornecedorService
    {
        private readonly IFornecedorRepository _repositoryBase;
        public FornecedorService(IFornecedorRepository repositoryBase) : base(repositoryBase)
        {
            _repositoryBase = repositoryBase;
        }
    }
}
