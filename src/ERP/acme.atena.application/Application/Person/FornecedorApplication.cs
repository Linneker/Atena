using acme.atena.application.Interface.Person;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Person
{
    public class FornecedorApplication : ServiceBase<Fornecedor>, IFornecedorApplication
    {
        private readonly IFornecedorService _fornecedorService;
        public FornecedorApplication(IFornecedorService repositoryBase) : base(repositoryBase)
        {
            _fornecedorService = repositoryBase;
        }
    }
}
