using acme.atena.api.ViewModel.Product.Price;
using acme.atena.application.Interface;
using acme.atena.application.Interface.Product;
using acme.atena.application.Interface.Product.Price;
using acme.atena.domain.DTO.Product.Price;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace acme.atena.api.Controllers.Product.Price
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoValorProdutoController : BaseApiController<TipoValorProduto, TipoValorProdutoViewModel>
    {
        private readonly ITipoValorProdutoApplication _tipoProdutoApplication;

        public TipoValorProdutoController(IMapper mapper, ITipoValorProdutoApplication tipoProdutoApplication) : base(mapper, tipoProdutoApplication, "Tipo Valor Produto")
        {
            _tipoProdutoApplication = tipoProdutoApplication; 
        }

        [HttpGet("Nome/{nome}")]
        [Authorize("Bearer")]
        public Task<TipoValorProduto> GetTipoValorProdutoByNomeAsync(string nome) =>
            _tipoProdutoApplication.GetTipoValorProdutoByNomeAsync(nome);

    }
}
