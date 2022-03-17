using acme.atena.application.Interface.Inventory;
using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.Interface.Service.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Inventory
{
    public class EstoqueProdutoApplication: ServiceBase<EstoqueProduto>, IEstoqueProdutoApplication
    {

        private readonly IEstoqueProdutoService _estoqueProdutoService;

        public EstoqueProdutoApplication(IEstoqueProdutoService estoqueProdutoService):base(estoqueProdutoService)
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
