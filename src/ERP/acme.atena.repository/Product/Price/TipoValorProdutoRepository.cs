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
    public class TipoValorProdutoRepository : RepositoryBase<TipoValorProduto>, ITipoValorProdutoRepository
    {
        public TipoValorProdutoRepository(Context db) : base(db)
        {
        }

        public Task<TipoValorProduto> GetTipoValorProdutoByNomeAsync(string nome)
        {
            var query = (from tvp in _db.TipoValorProdutos
                         where tvp.Nome.Equals(nome)
                         select tvp).AsNoTracking().FirstOrDefaultAsync();
            return query;
        }
    }
}
