using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.application.Interface.Person;
using acme.cashflow.domain.DTO.Person;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.Controllers.Person
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : BaseApiController<Pessoa,PessoaViewModel>
    {
        private readonly IPessoaApplication _pessoaApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "PESSOA";

        public PessoaController(IPessoaApplication pessoaApplication, IMapper mapper): base(mapper,pessoaApplication,NOME_SERVICO)
        {
            _pessoaApplication = pessoaApplication;
            _mapper = mapper;
        }
    }
}
