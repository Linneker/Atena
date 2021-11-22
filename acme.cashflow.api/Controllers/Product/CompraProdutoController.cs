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
    public class CompraProdutoController : BaseApiController<CompraProduto,CompraProdutoViewModel>
    {
        private readonly ICompraProdutoApplication _compraProdutoApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "COMPRA PRODUTO";

        public CompraProdutoController(ICompraProdutoApplication compraProdutoApplication, IMapper mapper): base(mapper,compraProdutoApplication,NOME_SERVICO)
        {
            _compraProdutoApplication = compraProdutoApplication;
            _mapper = mapper;
        }
    }
}
