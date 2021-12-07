using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.infra.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Inventory
{
    public class EstoqueRepository : RepositoryBase<Estoque>, IEstoqueRepository
    {
        public EstoqueRepository(Context db) : base(db)
        {
        }

        public List<Estoque> GetEstoquesByEmpressId(Guid empresaId)
        {
            List<Estoque> estoques = (from etq in _db.Estoques
                                      where etq.EmpresaId.Equals(empresaId)
                                      orderby etq.IsPrincipal descending
                                      select etq).AsNoTracking().ToList();
            return estoques;
        }

        public Task<List<Estoque>> GetEstoquesByEmpressIdAsync(Guid empresaId)
        {

            Task<List<Estoque>> estoques = (from etq in _db.Estoques
                                            where etq.EmpresaId.Equals(empresaId)
                                            orderby etq.IsPrincipal descending
                                            select etq).AsNoTracking().ToListAsync();
            return estoques;
        }
    }
}
