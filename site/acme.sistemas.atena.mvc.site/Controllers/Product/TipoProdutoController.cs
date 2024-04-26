using acme.atena.application.Handler.TipoProdutos.Commands;
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
                    Descricao = tipoProduto.Descricao,
                    Nome = tipoProduto.Nome
                });
            }
            return View(commandList);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        [UnitOfWorkFilterAsync]
        [HttpPost]
        public async Task<IActionResult> Criar(TipoProdutoCommand tipoProdutoCommand)
        {
            await _tipoProdutoService.AddAsync(new acme.atena.domain.DTO.Product.TipoProduto() { Nome = tipoProdutoCommand.Nome, Descricao = tipoProdutoCommand.Descricao });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Editar(Guid id)
        {
            var edicao = await _tipoProdutoService.GetByIdAsync(id);
            return View(new TipoProdutoCommand() { Descricao = edicao.Descricao, Nome = edicao.Nome});
        }

        [UnitOfWorkFilterAsync]
        [HttpPost]
        public IActionResult Editar(Guid id, TipoProdutoCommand tipoProdutoCommand)
        {
             _tipoProdutoService.Update(new acme.atena.domain.DTO.Product.TipoProduto() {Id = id, Nome = tipoProdutoCommand.Nome, Descricao = tipoProdutoCommand.Descricao });
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Detalhes(Guid id)
        {
            var edicao = await _tipoProdutoService.GetByIdAsync(id);
            return View(new TipoProdutoCommand() {Id= id, Descricao = edicao.Descricao, Nome = edicao.Nome });
        }

        public async Task<IActionResult> Remover(Guid id)
        {
            var edicao = await _tipoProdutoService.GetByIdAsync(id);
            return View(new TipoProdutoCommand() { Descricao = edicao.Descricao, Nome = edicao.Nome });
        }
        [UnitOfWorkFilterAsync]
        [HttpPost]
        public async Task<IActionResult> Remover(Guid id, TipoProdutoCommand tipoProdutoCommand)
        {
            var remover = await _tipoProdutoService.GetByIdAsync(id);
            _tipoProdutoService.Delete(remover);
            return RedirectToAction(nameof(Index));
        }
    }
}
