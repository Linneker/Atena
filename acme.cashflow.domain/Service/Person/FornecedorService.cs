using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Person;
using acme.cashflow.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Person
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
