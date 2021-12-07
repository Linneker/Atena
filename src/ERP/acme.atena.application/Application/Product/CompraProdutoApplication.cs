using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Product
{
    public class CompraProdutoApplication : ApplicationBase<CompraProduto>, ICompraProdutoApplication
    {
        private readonly ICompraProdutoService _compraProdutoService;
        public CompraProdutoApplication(ICompraProdutoService compraProdutoService):base(compraProdutoService)
        {
            _compraProdutoService = compraProdutoService;
        }

        public void Recebido(CompraProduto compraProduto, Guid estoqueId)
        {
            _compraProdutoService.Recebido(compraProduto, estoqueId);
        }
    }
}
