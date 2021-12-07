using acme.atena.domain.DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Inventory
{
    public interface IEntradaProdutoEstoqueService : IServiceBase<EntradaProdutoEstoque>
    {
        EntradaProdutoEstoque GetByProdutoId(Guid produtoId);
        Task<EntradaProdutoEstoque> GetByProdutoIdAsync(Guid produtoId);

    }
}
