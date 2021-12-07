using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Product
{
    public class CompraApplication: ApplicationBase<Compra>, ICompraApplication
    {
        private readonly ICompraService _compraService;

        public CompraApplication(ICompraService compraService):base(compraService)
        {
            _compraService = compraService;
        }

        public void CadastroAsync(Compra compra, Guid empresaId)
        {
            _compraService.CadastroAsync(compra, empresaId);
        }
    }
}
