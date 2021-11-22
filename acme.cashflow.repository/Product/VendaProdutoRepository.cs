using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Product
{
    public class VendaProdutoRepository : RepositoryBase<VendaProduto>, IVendaProdutoRepository
    {
        public VendaProdutoRepository(Context db) : base(db)
        {
        }
    }
}
