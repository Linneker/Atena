using acme.cashflow.application.Interface.Product;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Product
{
    public class CompraApplication: ApplicationBase<Compra>, ICompraApplication
    {
        private readonly ICompraService _compraService;

        public CompraApplication(ICompraService compraService):base(compraService)
        {
            _compraService = compraService;
        }
    }
}
