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
    public class SaidaProdutoEstoqueService : ServiceBase<SaidaProdutoEstoque>, ISaidaProdutoEstoqueService
    {
        private readonly ISaidaProdutoEstoqueRepository _saidaProdutoEstoqueRepository;

        public SaidaProdutoEstoqueService(ISaidaProdutoEstoqueRepository saidaProdutoEstoqueRepository):base(saidaProdutoEstoqueRepository)
        {
            _saidaProdutoEstoqueRepository = saidaProdutoEstoqueRepository;
        }
    }
}
