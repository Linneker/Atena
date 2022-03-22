using acme.atena.domain.DTO.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Product.Price
{
    public interface IValorProdutoService : IServiceBase<ValorProduto>
    {
        Task<ValorProduto> GetValorProdutoByProdutoIdAsync(Guid produtoId);
    }
}
