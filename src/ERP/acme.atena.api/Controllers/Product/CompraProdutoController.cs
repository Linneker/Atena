using acme.atena.api.ViewModel.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraProdutoController : BaseApiController<CompraProduto,CompraProdutoViewModel>
    {
        private readonly ICompraProdutoService _compraProdutoApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "COMPRA PRODUTO";

        public CompraProdutoController(ICompraProdutoService compraProdutoApplication, IMapper mapper): base(mapper,compraProdutoApplication,NOME_SERVICO)
        {
            _compraProdutoApplication = compraProdutoApplication;
            _mapper = mapper;
        }
    }
}
