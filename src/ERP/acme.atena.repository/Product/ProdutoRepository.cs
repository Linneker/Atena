using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace acme.atena.repository.Product
{
    public class ProdutoRepository : RepositoryBase<Produto>, IProdutoRepository
    {
        public ProdutoRepository(Context db) : base(db)
        {
        }

        public Produto GetProdutoJoinEstoqueProdutoById(Guid id)
        {
            Produto produto = (from prd in _db.Produtos
                                     join estProd in _db.EstoqueProdutos on prd.Id equals estProd.ProdutoId
                                     where prd.Id == id
                                     select prd).Include(t => t.EstoqueProdutos).AsNoTracking().FirstOrDefault();
            return produto;
        }

        public Task<Produto> GetProdutoJoinEstoqueProdutoByIdAsync(Guid id)
        {
            Task<Produto>  produto = (from prd in _db.Produtos
                                      join estProd in _db.EstoqueProdutos on prd.Id equals estProd.ProdutoId
                                      where prd.Id == id
                                      select prd).Include(t=>t.EstoqueProdutos).AsNoTracking().FirstOrDefaultAsync();
            return produto;
        }


        public long GetTotalProdutoByEstoqueId(Guid estoqueId)
        {
            long total = (from prd in _db.Produtos
                          join estProd in _db.EstoqueProdutos on prd.Id equals estProd.ProdutoId
                          where estProd.EstoqueId == estoqueId
                          select estProd).Sum(t => t.QuantidadeProduto);
            return total;
        }

        public  async Task<Produto> ObterProdutosPeloId(Guid id)
        {
            var produto = await (from prd in _db.Produtos
                                     join tp in _db.TipoProduto on prd.TipoProdutoId equals tp.Id
                                     where prd.Id == id
                                     select prd).Include(t => t.TipoProduto).AsNoTracking().FirstOrDefaultAsync();
            return produto;
        }

        public async Task<List<Produto>> ObterProdutosJoinTipoProduto()
        {
            var produto = await (from prd in _db.Produtos
                                     join tp in _db.TipoProduto on prd.TipoProdutoId equals tp.Id
                                     select prd).Include(t => t.TipoProduto).AsNoTracking().ToListAsync();
            return produto;
        }

    }
}
