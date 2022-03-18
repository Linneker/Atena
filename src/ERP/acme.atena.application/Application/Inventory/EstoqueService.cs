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
    public class EstoqueService : ServiceBase<Estoque>, IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueService;

        public EstoqueService(IEstoqueRepository estoqueService):base(estoqueService)
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
