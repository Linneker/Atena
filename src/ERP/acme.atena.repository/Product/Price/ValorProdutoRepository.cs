using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.Interface.Repository.Product.Price;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Product.Price
{
    public class ValorProdutoRepository : RepositoryBase<ValorProduto>, IValorProdutoRepository
    {
        public ValorProdutoRepository(Context db) : base(db)
        {
        }

        public Task<ValorProduto> GetValorProdutoByProdutoIdAsync(Guid produtoId)
        {
            Task<ValorProduto> query = (from vp in _db.ValorProduto where vp.Id.Equals(produtoId) select vp).AsNoTracking().FirstOrDefaultAsync();
            return query;
        }
    }
}
