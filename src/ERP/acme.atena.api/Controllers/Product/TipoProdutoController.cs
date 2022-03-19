using acme.atena.api.ViewModel.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace acme.atena.api.Controllers.Product
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoProdutoController : BaseApiController<TipoProduto, TipoProdutoViewModel>
    {
        private readonly IMapper _mapper;
        private readonly ITipoProdutoService _serviceBase;
        public TipoProdutoController(IMapper mapper, ITipoProdutoService applicationBase) : base(mapper, applicationBase, "TipoProduto")
        {
            _mapper = mapper;
            _serviceBase = applicationBase;
        }
    }
}
