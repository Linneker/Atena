using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Inventory
{
    public class EstoqueProdutoRepository : RepositoryBase<EstoqueProduto>, IEstoqueProdutoRepository
    {
        public EstoqueProdutoRepository(Context db) : base(db)
        {
        }

        public EstoqueProduto GetEstoqueProdutoByProdutoId(Guid produtoId)
        {
            EstoqueProduto estoqueProduto = (from estcProd in _db.EstoqueProdutos
                                             where estcProd.ProdutoId == produtoId
                                             select estcProd).AsNoTracking().FirstOrDefault();
            return estoqueProduto;
        }

        public Task<EstoqueProduto> GetEstoqueProdutoByProdutoIdAsync(Guid produtoId)
        {
            Task<EstoqueProduto> estoqueProduto = (from estcProd in _db.EstoqueProdutos
                                             where estcProd.ProdutoId == produtoId
                                             select estcProd).AsNoTracking().FirstOrDefaultAsync();
            return estoqueProduto;
        }
    }
}
