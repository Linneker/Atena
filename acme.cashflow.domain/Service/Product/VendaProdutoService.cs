using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Product
{
    public class VendaProdutoService : ServiceBase<VendaProduto>, IVendaProdutoService
    {
        private readonly IVendaProdutoRepository _vendaProdutoRepository;

        public VendaProdutoService(IVendaProdutoRepository vendaProdutoRepository) : base(vendaProdutoRepository)
        {
            _vendaProdutoRepository = vendaProdutoRepository;
        }
    }
}
