using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Repository.Person;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Person;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Person
{
    public class FornecedorService : ServiceBase<Fornecedor>, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorService;
        public FornecedorService(IFornecedorRepository repositoryBase) : base(repositoryBase)
        {
            _fornecedorService = repositoryBase;
        }
    }
}
