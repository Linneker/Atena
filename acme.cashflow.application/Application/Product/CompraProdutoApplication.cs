using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Product
{
    public class CompraProdutoApplication : ApplicationBase<CompraProduto>, ICompraProdutoApplication
    {
        private readonly ICompraProdutoService _compraProdutoService;
        public CompraProdutoApplication(ICompraProdutoService compraProdutoService):base(compraProdutoService)
        {
            _compraProdutoService = compraProdutoService;
        }
    }
}
