using acme.atena.application.Handler.TipoProdutos.Command;
using acme.atena.domain.Interface.Service.Product;
using acme.sistemas.atena.mvc.site.Filtler;
using Microsoft.AspNetCore.Mvc;

namespace acme.sistemas.atena.mvc.site.Controllers.Product
{
    public class TipoProdutoController : Controller
    {
        private readonly ITipoProdutoService _tipoProdutoService;

        public TipoProdutoController(ITipoProdutoService tipoProdutoService)
        {
            _tipoProdutoService = tipoProdutoService;
        }

        public async Task<IActionResult> Index()
        {
            var tiposProdutos = await _tipoProdutoService.GetAllAsync();
            List<TipoProdutoCommand> commandList = new List<TipoProdutoCommand>();  
            foreach (var tipoProduto in tiposProdutos)
            {
                commandList.Add(new TipoProdutoCommand()
                {
                    Id = tipoProduto.Id,
                    Descricao = tipoProduto.Nome,
                    Nome = tipoProduto.Nome
                });
            }
            return View(commandList);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            return View();
        }

        [UnitOfWorkFilterAsync]
        [HttpPost]
        public async Task<IActionResult> Criar(TipoProdutoCommand tipoProdutoCommand)
        {
            await _tipoProdutoService.AddAsync(new acme.atena.domain.DTO.Product.TipoProduto() { Nome = tipoProdutoCommand.Nome, Descricao = tipoProdutoCommand.Descricao });
            return View();
        }

    }
}
