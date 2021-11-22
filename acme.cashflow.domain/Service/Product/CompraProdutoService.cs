using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Product
{
    public class CompraProdutoService: ServiceBase<CompraProduto>, ICompraProdutoService
    {
        private readonly ICompraProdutoRepository _compraProdutoRepository;

        public CompraProdutoService(ICompraProdutoRepository repositoryBase) : base(repositoryBase)
        {
            _compraProdutoRepository = repositoryBase;
        }
    }
}
