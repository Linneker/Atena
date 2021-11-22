using acme.cashflow.api.ViewModel.Product;
using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.cashflow.api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendaController : BaseApiController<Venda,VendaViewModel>
    {
        private readonly IVendaApplication _vendaApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "VENDA";

        public VendaController(IVendaApplication vendaApplication, IMapper mapper):base(mapper,vendaApplication, NOME_SERVICO)
        {
            _vendaApplication = vendaApplication;
            _mapper = mapper;
        }
    }
}
