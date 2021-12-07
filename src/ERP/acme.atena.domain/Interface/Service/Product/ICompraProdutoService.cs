using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Interface.Service.Product
{
    public interface ICompraProdutoService : IServiceBase<CompraProduto>
    {
        void Recebido(CompraProduto compraProduto, Guid estoqueId);
        
    }
}
