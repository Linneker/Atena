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
    public class SaidaProdutoEstoqueApplication : ServiceBase<SaidaProdutoEstoque>, ISaidaProdutoEstoqueApplication
    {
        private readonly ISaidaProdutoEstoqueService _saidaProdutoEstoqueService;

        public SaidaProdutoEstoqueApplication(ISaidaProdutoEstoqueService saidaProdutoEstoqueService):base(saidaProdutoEstoqueService)
        {
            _saidaProdutoEstoqueService = saidaProdutoEstoqueService;
        }
    }
}
