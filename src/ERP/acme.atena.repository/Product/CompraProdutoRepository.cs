using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.repository.Product
{
    public class CompraProdutoRepository : RepositoryBase<CompraProduto>, ICompraProdutoRepository
    {
        public CompraProdutoRepository(Context db) : base(db)
        {
        }
    }
}
