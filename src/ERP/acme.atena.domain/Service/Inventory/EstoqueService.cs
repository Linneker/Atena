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
    public class EstoqueService : ServiceBase<Estoque>, IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;

        public EstoqueService(IEstoqueRepository estoqueRepository):base(estoqueRepository)
        {
            _estoqueRepository = estoqueRepository;
        }

        public List<Estoque> GetEstoquesByEmpressId(Guid empresaId)
        {
            return _estoqueRepository.GetEstoquesByEmpressId(empresaId);
        }

        public Task<List<Estoque>> GetEstoquesByEmpressIdAsync(Guid empresaId)
        {
            return _estoqueRepository.GetEstoquesByEmpressIdAsync(empresaId);
        }
    }
}
