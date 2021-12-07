using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Product
{
    public class VendaApplication: ApplicationBase<Venda>,IVendaApplication
    {
        private readonly IVendaService _vendaService;

        public VendaApplication(IVendaService vendaService):base(vendaService)
        {
            _vendaService = vendaService;
        }

        public void Conclusao(Venda venda, Guid empresaId)
        {
            _vendaService.Conclusao(venda, empresaId);
        }
    }
}
