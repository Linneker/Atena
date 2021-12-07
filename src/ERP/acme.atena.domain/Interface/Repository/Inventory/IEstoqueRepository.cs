using acme.atena.domain.DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Repository.Inventory
{
    public interface IEstoqueRepository : IRepositoryBase<Estoque>
    {
        List<Estoque> GetEstoquesByEmpressId(Guid empresaId);
        Task<List<Estoque>> GetEstoquesByEmpressIdAsync(Guid empresaId);
    }
}
