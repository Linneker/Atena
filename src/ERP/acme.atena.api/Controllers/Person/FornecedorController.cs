using acme.atena.api.ViewModel.Person;
using acme.atena.application.Interface.Person;
using acme.atena.domain.DTO.Person;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Person
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedorController : BaseApiController<Fornecedor,FornecedorViewModel>
    {
        private readonly IFornecedorApplication _fornecedorApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "FORNECEDOR";

        public FornecedorController(IFornecedorApplication fornecedorApplication, IMapper mapper): base(mapper, fornecedorApplication,NOME_SERVICO)
        {
            _fornecedorApplication = fornecedorApplication;
            _mapper = mapper;
        }
    }
}
