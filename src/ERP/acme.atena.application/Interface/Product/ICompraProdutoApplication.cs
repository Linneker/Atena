using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Interface.Product
{
    public interface ICompraProdutoApplication : IApplicationBase<CompraProduto>
    {
        void Recebido(CompraProduto compraProduto, Guid estoqueId);
        
    }
}
