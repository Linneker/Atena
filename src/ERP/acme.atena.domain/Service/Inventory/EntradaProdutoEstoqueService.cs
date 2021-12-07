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
    public class EntradaProdutoEstoqueService : ServiceBase<EntradaProdutoEstoque>, IEntradaProdutoEstoqueService
    {
        private readonly IEntradaProdutoEstoqueRepository _entradaProdutoEstoqueRepository;

        public EntradaProdutoEstoqueService(IEntradaProdutoEstoqueRepository entradaProdutoEstoqueRepository):base(entradaProdutoEstoqueRepository)
        {
            _entradaProdutoEstoqueRepository = entradaProdutoEstoqueRepository;
        }

        public EntradaProdutoEstoque GetByProdutoId(Guid produtoId)
        {
            return _entradaProdutoEstoqueRepository.GetByProdutoId(produtoId);
        }

        public Task<EntradaProdutoEstoque> GetByProdutoIdAsync(Guid produtoId)
        {
            return _entradaProdutoEstoqueRepository.GetByProdutoIdAsync(produtoId);
        }
    }
}
