using AutoMapper;
using acme.atena.api.ViewModel.Account;
using acme.atena.application.Interface.Util;
using acme.atena.domain.DTO.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class FluxoDeCaixaController : BaseApiController<FluxoDeCaixa, FluxoDeCaixaViewModel>
    {
        private IMapper _mapper;
        private IFluxoDeCaixaApplication _despesaApplication;
        private const string NOME_SERVICO = "FLUXO DE CAIXA";
        public FluxoDeCaixaController(IMapper mapper, IFluxoDeCaixaApplication despesaApplication) : base(mapper, despesaApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _despesaApplication = despesaApplication;
        }
    }
}
