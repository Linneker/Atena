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
    public class EstoqueApplication : ServiceBase<Estoque>, IEstoqueApplication
    {
        private readonly IEstoqueService _estoqueService;

        public EstoqueApplication(IEstoqueService estoqueService):base(estoqueService)
        {
            _estoqueService = estoqueService;
        }

        public List<Estoque> GetEstoquesByEmpressId(Guid empresaId)
        {
            return _estoqueService.GetEstoquesByEmpressId(empresaId);
        }

        public Task<List<Estoque>> GetEstoquesByEmpressIdAsync(Guid empresaId)
        {
            return _estoqueService.GetEstoquesByEmpressIdAsync(empresaId);
        }
    }
}
