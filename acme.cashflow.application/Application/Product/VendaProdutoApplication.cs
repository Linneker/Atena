using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Product
{
    public class VendaProdutoApplication: ApplicationBase<VendaProduto>, IVendaProdutoApplication
    {
        private readonly IVendaProdutoService _vendaProdutoService;

        public VendaProdutoApplication(IVendaProdutoService vendaProdutoService):base(vendaProdutoService)
        {
            _vendaProdutoService = vendaProdutoService;
        }
    }
}
