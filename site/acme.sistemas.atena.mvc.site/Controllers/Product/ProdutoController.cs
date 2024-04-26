using acme.atena.application.Handler.Produtos.Commands.Request;
using acme.atena.application.Handler.Produtos.Commands.Response;
using acme.atena.application.Handler.Produtos.Query;
using acme.atena.domain.DTO.Product.Sequence;
using acme.atena.domain.Interface.Repository.UnitOfWork;
using acme.atena.domain.Interface.Service.Product;
using acme.sistemas.atena.mvc.site.Filtler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace acme.sistemas.atena.mvc.site.Controllers.Product
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoService _produtoService;
        private readonly ITipoProdutoService _tipProdutoService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProtudoQuery _protudoQuery;
        public ProdutoController(
            IProdutoService produtoService,
            ITipoProdutoService tipProdutoService,
            IUnitOfWork unitOfWork,
            ProtudoQuery protudoQuery)
        {
            _produtoService = produtoService;
            _tipProdutoService = tipProdutoService;
            _unitOfWork = unitOfWork;
            _protudoQuery = protudoQuery;
        }

        public async Task<IActionResult> Index()
        {
            var produtos = await _protudoQuery.ObterProdutosJoinTipoProduto();
            return View(produtos);
        }

        public IActionResult Criar()
        {
            var tipoProdutos = _tipProdutoService.GetAll();
            var valores = new SelectList(tipoProdutos, "Id", "Nome");
            ViewBag.DropDownTipoProduto = valores;
            return View();

        }
        [UnitOfWorkFilterAsync]
        [HttpPost]
        public async Task<IActionResult> Criar(InputProdutoCommand produtoCommand)
        {
            var produto = new acme.atena.domain.DTO.Product.Produto() { Nome = produtoCommand.Nome, Descricao = produtoCommand.Descricao, Validade = produtoCommand.Validade, TipoProdutoId = produtoCommand.TipoProdutoId, CodigoProduto = new CodigoProduto() };
            await _produtoService.AddAsync(produto); ;
            return RedirectToAction(nameof(Index));
        }

        public  IActionResult Editar(Guid id)
        {
            var tipoProdutos = _tipProdutoService.GetAll();
            var produto = _produtoService.GetById(id);
            var valores = new SelectList(tipoProdutos, "Id", "Nome", tipoProdutos.Where(t => t.Id.Equals(produto.Id)).FirstOrDefault());
            ViewBag.DropDownTipoProduto = valores;
            return View(new InputProdutoCommand() { Descricao = produto.Descricao, Nome = produto.Nome, TipoProdutoId = produto.TipoProdutoId, Validade = produto.Validade });

        }

        [HttpPost]
        public async Task<IActionResult> Editar(Guid id, InputProdutoCommand produtoCommand)
        {
            var produto = _produtoService.GetById(id);
            var novoProduto = new acme.atena.domain.DTO.Product.Produto() { Id = id, Nome = produtoCommand.Nome, Descricao = produtoCommand.Descricao, Validade = produtoCommand.Validade, TipoProdutoId = produtoCommand.TipoProdutoId };
            bool temProdutoAtualizar = produto.Atualizar(novoProduto);
            if (temProdutoAtualizar)
            {
                _produtoService.Update(produto);
                bool salvo = await _unitOfWork.CommitAsync();
                if (salvo)
                    return RedirectToAction(nameof(Index));
                else
                {
                    return View();
                }
            }
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Detalhes(Guid id)
        {
            var edicao = await _protudoQuery.ObterProdutoPeloId(id);
            return View(edicao);
        }

        public async Task<IActionResult> Remover(Guid id)
        {
            var edicao = await _protudoQuery.ObterProdutoPeloId(id);
            return View(edicao);
        }
        [UnitOfWorkFilterAsync]
        [HttpPost]
        public async Task<IActionResult> Remover(Guid id, OutputProdutoCommand produtoCommand)
        {
            var remover = await _produtoService.GetByIdAsync(id);
            _produtoService.Delete(remover);
            return RedirectToAction(nameof(Index));
        }
    }
}
