using acme.atena.api.ViewModel.Person;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.Interface.Service.Person;
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
    public class PessoaController : BaseApiController<Cliente, ClienteViewModel>
    {
        private readonly IClienteService _pessoaApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "PESSOA";

        public PessoaController(IClienteService pessoaApplication, IMapper mapper): base(mapper,pessoaApplication,NOME_SERVICO)
        {
            _pessoaApplication = pessoaApplication;
            _mapper = mapper;
        }
    }
}
