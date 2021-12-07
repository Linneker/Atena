using acme.atena.domain.DTO.Inventory;
using acme.atena.domain.Interface.Repository.Inventory;
using acme.atena.infra.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.repository.Inventory
{
    public class SaidaProdutoEstoqueRepository : RepositoryBase<SaidaProdutoEstoque>, ISaidaProdutoEstoqueRepository
    {
        public SaidaProdutoEstoqueRepository(Context db) : base(db)
        {
        }
    }
}
