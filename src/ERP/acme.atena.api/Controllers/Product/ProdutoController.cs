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
    public class ProdutoController : BaseApiController<Produto, ProdutoViewModel>
    {
        private readonly IProdutoService _produtoApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "PRODUTO";

        public ProdutoController(IProdutoService produtoApplication, IMapper mapper):base(mapper,produtoApplication,NOME_SERVICO)
        {
            _produtoApplication = produtoApplication;
            _mapper = mapper;
        }
    }
}
