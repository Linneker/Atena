using acme.atena.domain.DTO.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Product.Price
{
    public interface ITipoValorProdutoApplication: IApplicationBase<TipoValorProduto>
    {
        Task<TipoValorProduto> GetTipoValorProdutoByNomeAsync(string nome);
    }
}
