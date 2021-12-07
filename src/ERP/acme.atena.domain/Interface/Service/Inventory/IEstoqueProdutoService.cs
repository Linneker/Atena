using acme.atena.domain.DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Inventory
{
    public interface IEstoqueProdutoService: IServiceBase<EstoqueProduto>
    {
        EstoqueProduto GetEstoqueProdutoByProdutoId(Guid produtoId);
        Task<EstoqueProduto> GetEstoqueProdutoByProdutoIdAsync(Guid produtoId);
    }
}
