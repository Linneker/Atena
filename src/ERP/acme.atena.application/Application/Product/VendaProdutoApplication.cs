using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Product
{
    public class VendaProdutoApplication: ApplicationBase<VendaProduto>, IVendaProdutoApplication
    {
        private readonly IVendaProdutoService _vendaProdutoService;

        public VendaProdutoApplication(IVendaProdutoService vendaProdutoService):base(vendaProdutoService)
        {
            _vendaProdutoService = vendaProdutoService;
        }

        public void Enviado(VendaProduto vendaProduto, Guid estoqueId)
        {
            _vendaProdutoService.Enviado(vendaProduto, estoqueId);
        }
    }
}
