using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Repository.Product
{
    public interface IProdutoRepository : IRepositoryBase<Produto>
    {
        long GetTotalProdutoByEstoqueId(Guid estoqueId);
        Produto GetProdutoJoinEstoqueProdutoById(Guid id);
        Task<Produto> GetProdutoJoinEstoqueProdutoByIdAsync(Guid id);
        Task<Produto> ObterProdutosPeloId(Guid id);
        Task<List<Produto>> ObterProdutosJoinTipoProduto();

    }
}
