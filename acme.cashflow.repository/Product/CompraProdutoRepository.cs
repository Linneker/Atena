using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Product
{
    public class CompraProdutoRepository : RepositoryBase<CompraProduto>, ICompraProdutoRepository
    {
        public CompraProdutoRepository(Context db) : base(db)
        {
        }
    }
}
