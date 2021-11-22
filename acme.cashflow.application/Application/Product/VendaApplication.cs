using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Product
{
    public class VendaApplication: ApplicationBase<Venda>,IVendaApplication
    {
        private readonly IVendaService _vendaService;

        public VendaApplication(IVendaService vendaService):base(vendaService)
        {
            _vendaService = vendaService;
        }
    }
}
