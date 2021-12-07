using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.DTO.Product;
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
    public class EntradaProdutoEstoqueRepository : RepositoryBase<EntradaProdutoEstoque>, IEntradaProdutoEstoqueRepository
    {
        public EntradaProdutoEstoqueRepository(Context db) : base(db)
        {
        }

        public EntradaProdutoEstoque GetByProdutoId(Guid produtoId)
        {
            EntradaProdutoEstoque prod = (from entProd in _db.EntradaProdutoEstoques
                            where entProd.ProdutoId.Equals(produtoId)
                            select entProd).FirstOrDefault();
            return prod;
        }

        public Task<EntradaProdutoEstoque> GetByProdutoIdAsync(Guid produtoId)
        {
            Task<EntradaProdutoEstoque> prod = (from entProd in _db.EntradaProdutoEstoques
                                          where entProd.ProdutoId.Equals(produtoId)
                                          select entProd).FirstOrDefaultAsync();
            return prod;
        }
    }
}
