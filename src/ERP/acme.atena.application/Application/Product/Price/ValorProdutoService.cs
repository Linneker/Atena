using acme.atena.domain.DTO.Product.Price;
using acme.atena.domain.Interface.Repository;
using acme.atena.domain.Interface.Repository.Product.Price;
using acme.atena.domain.Interface.Service.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service.Product.Price
{
    public class ValorProdutoService: ServiceBase<ValorProduto>, IValorProdutoService
    {
        private readonly IValorProdutoRepository _valorProdutoRepository;

        public ValorProdutoService(IValorProdutoRepository valorProdutoRepository) : base(valorProdutoRepository)
        {
            _valorProdutoRepository = valorProdutoRepository;
        }

        public Task<ValorProduto> GetValorProdutoByProdutoIdAsync(Guid produtoId)
        {
            return _valorProdutoRepository.GetValorProdutoByProdutoIdAsync(produtoId);
        }
    }
}
