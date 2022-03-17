using acme.atena.api.ViewModel.Product;
using acme.atena.application.Interface;
using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
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

        public TipoProdutoController(IMapper mapper, ITipoProdutoApplication  applicationBase) : base(mapper, applicationBase, "TipoProduto")
        {
            _mapper = mapper;
        }
    }
}
