using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.infra.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.repository.Product
{
    public class VendaRepository : RepositoryBase<Venda>, IVendaRepository
    {
        public VendaRepository(Context db) : base(db)
        {
        }
    }
}
