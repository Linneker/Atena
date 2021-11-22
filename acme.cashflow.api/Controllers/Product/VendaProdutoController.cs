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
    public class VendaProdutoController : BaseApiController<VendaProduto,VendaProdutoViewModel>
    {
        private readonly IVendaProdutoApplication _vendaProdutoApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "VENDA PRODUTO";

        public VendaProdutoController(IVendaProdutoApplication vendaProdutoApplication, IMapper mapper)
            :base(mapper, vendaProdutoApplication, NOME_SERVICO)
        {
            _vendaProdutoApplication = vendaProdutoApplication;
            _mapper = mapper;
        }
    }
}
