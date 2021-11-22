using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Product
{
    public class ProdutoService : ServiceBase<Produto>,IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository):base(produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }
    }
}
