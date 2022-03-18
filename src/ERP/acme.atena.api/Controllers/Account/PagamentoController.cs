using acme.atena.api.ViewModel.Account;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Service.Account;
using AutoMapper;
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
    public class PagamentoController : BaseApiController<Pagamento,PagamentoViewModel>
    {
        private readonly IMapper _mapper;
        private readonly IPagamentoService _pagamentoApplication;
        private const string NOME_SERVICO = "PAGAMENTO";

        public PagamentoController(IMapper mapper, IPagamentoService pagamentoApplication):base(mapper, pagamentoApplication, NOME_SERVICO)
        {
            _mapper = mapper;
            _pagamentoApplication = pagamentoApplication;
        }
    }
}
