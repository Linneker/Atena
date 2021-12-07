using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.Interface.Repository.Product.Price;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Product.Price
{
    public class TipoValorProdutoRepository : RepositoryBase<TipoValorProduto>, ITipoValorProdutoRepository
    {
        public TipoValorProdutoRepository(Context db) : base(db)
        {
        }
    }
}
