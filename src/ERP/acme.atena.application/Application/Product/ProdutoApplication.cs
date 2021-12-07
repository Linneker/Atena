using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Product
{
    public class ProdutoApplication : ApplicationBase<Produto>,IProdutoApplication
    {
        private readonly IProdutoService _produtoService;
        public ProdutoApplication(IProdutoService produtoService):base(produtoService)
        {
            _produtoService = produtoService;
        }

        public Produto GetProdutoJoinEstoqueProdutoById(Guid id)
        {
           return _produtoService.GetProdutoJoinEstoqueProdutoById(id);
        }

        public Task<Produto> GetProdutoJoinEstoqueProdutoByIdAsync(Guid id)
        {
            return _produtoService.GetProdutoJoinEstoqueProdutoByIdAsync(id);
        }

        public long GetTotalProdutoByEstoqueId(Guid estoqueId)
        {
            return _produtoService.GetTotalProdutoByEstoqueId(estoqueId);
        }
    }
}
