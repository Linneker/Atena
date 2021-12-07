using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.domain.Interface.Service.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Inventory
{
    public class EstoqueProdutoService : ServiceBase<EstoqueProduto>, IEstoqueProdutoService
    {
        private readonly IEstoqueProdutoRepository _estoqueProdutoRepository;

        public EstoqueProdutoService(IEstoqueProdutoRepository estoqueProdutoRepository):base(estoqueProdutoRepository)
        {
            _estoqueProdutoRepository = estoqueProdutoRepository;
        }

        public EstoqueProduto GetEstoqueProdutoByProdutoId(Guid produtoId)
        {
            return _estoqueProdutoRepository.GetEstoqueProdutoByProdutoId(produtoId);
        }

        public Task<EstoqueProduto> GetEstoqueProdutoByProdutoIdAsync(Guid produtoId)
        {
            return _estoqueProdutoRepository.GetEstoqueProdutoByProdutoIdAsync(produtoId);
        }
    }
}
