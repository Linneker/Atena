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
    public class EntradaProdutoEstoqueApplication : ApplicationBase<EntradaProdutoEstoque>, IEntradaProdutoEstoqueApplication
    {
        private readonly IEntradaProdutoEstoqueService _entradaProdutoEstoqueService;

        public EntradaProdutoEstoqueApplication(IEntradaProdutoEstoqueService entradaProdutoEstoqueService):base(entradaProdutoEstoqueService)
        {
            _entradaProdutoEstoqueService = entradaProdutoEstoqueService;
        }

        public EntradaProdutoEstoque GetByProdutoId(Guid produtoId)
        {
            return _entradaProdutoEstoqueService.GetByProdutoId(produtoId);        
        }

        public Task<EntradaProdutoEstoque> GetByProdutoIdAsync(Guid produtoId)
        {
            return _entradaProdutoEstoqueService.GetByProdutoIdAsync(produtoId);
        }
    }
}
