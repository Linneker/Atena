using acme.atena.domain.DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Inventory
{
    public interface IEntradaProdutoEstoqueApplication : IApplicationBase<EntradaProdutoEstoque>
    {
        EntradaProdutoEstoque GetByProdutoId(Guid produtoId);
        Task<EntradaProdutoEstoque> GetByProdutoIdAsync(Guid produtoId);

    }
}
