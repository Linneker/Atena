using acme.cashflow.application.Interface.Person;
using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.Interface.Service;
using acme.cashflow.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Person
{
    public class FornecedorApplication : ApplicationBase<Fornecedor>, IFornecedorApplication
    {
        private readonly IFornecedorService _fornecedorService;
        public FornecedorApplication(IFornecedorService repositoryBase) : base(repositoryBase)
        {
            _fornecedorService = repositoryBase;
        }
    }
}
