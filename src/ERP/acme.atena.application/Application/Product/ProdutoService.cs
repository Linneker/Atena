using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Product;
using acme.atena.domain.Interface.Service.Product.Price;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service.Product
{
    public class ProdutoService : ServiceBase<Produto>,IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IValorProdutoService _valorProdutoService;
        public ProdutoService(IProdutoRepository produtoRepository, IValorProdutoService valorProdutoService) :base(produtoRepository)
        {
            _produtoRepository = produtoRepository;
            _valorProdutoService = valorProdutoService;
        }

        public async override void Delete(Guid id)
        {
            var valorProduto = await _valorProdutoService.GetValorProdutoByProdutoIdAsync(id);
            _valorProdutoService.Delete(valorProduto);
            _produtoRepository.Delete(id);
        }

        public Produto GetProdutoJoinEstoqueProdutoById(Guid id)
        {
            return _produtoRepository.GetProdutoJoinEstoqueProdutoById(id);
        }

        public Task<Produto> GetProdutoJoinEstoqueProdutoByIdAsync(Guid id)
        {
            return _produtoRepository.GetProdutoJoinEstoqueProdutoByIdAsync(id);
        }

        public long GetTotalProdutoByEstoqueId(Guid estoqueId)
        {
            return _produtoRepository.GetTotalProdutoByEstoqueId(estoqueId);
        }
    }
}
