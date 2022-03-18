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
    public class EntradaProdutoEstoqueService : ServiceBase<EntradaProdutoEstoque>, IEntradaProdutoEstoqueService
    {
        private readonly IEntradaProdutoEstoqueRepository _entradaProdutoEstoqueService;

        public EntradaProdutoEstoqueService(IEntradaProdutoEstoqueRepository entradaProdutoEstoqueService):base(entradaProdutoEstoqueService)
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
