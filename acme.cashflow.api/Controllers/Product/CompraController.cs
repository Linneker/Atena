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
    public class CompraController : BaseApiController<Compra,CompraViewModel>
    {
        private readonly ICompraApplication _compraApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "COMPRA";

        public CompraController(ICompraApplication compraApplication, IMapper mapper): base(mapper,compraApplication,NOME_SERVICO)
        {
            _compraApplication = compraApplication;
            _mapper = mapper;
        }
    }
}
