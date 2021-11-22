using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Product
{
    public class ProdutoApplication : ApplicationBase<Produto>,IProdutoApplication
    {
        private readonly IProdutoService _produtoService;
        public ProdutoApplication(IProdutoService produtoService):base(produtoService)
        {
            _produtoService = produtoService;
        }
    }
}
