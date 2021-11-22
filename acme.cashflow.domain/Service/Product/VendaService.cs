using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Repository;
using acme.cashflow.domain.Interface.Repository.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.Service.Product
{
    public class VendaService : ServiceBase<Venda>, IVendaService
    {
        private readonly IVendaRepository _vendaRepository;

        public VendaService(IVendaRepository vendaRepository) : base(vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }
    }
}
