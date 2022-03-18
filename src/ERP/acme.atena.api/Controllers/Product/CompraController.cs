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
    public class CompraController : BaseApiController<Compra,CompraViewModel>
    {
        private readonly ICompraService _compraApplication;
        private readonly IMapper _mapper;
        private const string NOME_SERVICO = "COMPRA";

        public CompraController(ICompraService compraApplication, IMapper mapper): base(mapper,compraApplication,NOME_SERVICO)
        {
            _compraApplication = compraApplication;
            _mapper = mapper;
        }
    }
}
