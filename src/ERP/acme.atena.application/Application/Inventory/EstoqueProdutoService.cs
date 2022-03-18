using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.domain.Interface.Service.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service.Inventory
{
    public class EstoqueProdutoService: ServiceBase<EstoqueProduto>, IEstoqueProdutoService
    {

        private readonly IEstoqueProdutoRepository _estoqueProdutoService;

        public EstoqueProdutoService(IEstoqueProdutoRepository estoqueProdutoService):base(estoqueProdutoService)
        {
            _estoqueProdutoService = estoqueProdutoService;
        }

        public EstoqueProduto GetEstoqueProdutoByProdutoId(Guid produtoId)
        {
            return _estoqueProdutoService.GetEstoqueProdutoByProdutoId(produtoId);
        }

        public Task<EstoqueProduto> GetEstoqueProdutoByProdutoIdAsync(Guid produtoId)
        {
            return _estoqueProdutoService.GetEstoqueProdutoByProdutoIdAsync(produtoId);
        }
    }
}
