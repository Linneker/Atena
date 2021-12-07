using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Product
{
    public interface IProdutoService : IServiceBase<Produto>
    {
        long GetTotalProdutoByEstoqueId(Guid estoqueId);
        Produto GetProdutoJoinEstoqueProdutoById(Guid id);
        Task<Produto> GetProdutoJoinEstoqueProdutoByIdAsync(Guid id);

    }
}
