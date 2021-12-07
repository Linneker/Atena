using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Interface.Service.Product
{
    public interface ICompraService : IServiceBase<Compra>
    {
        void CadastroAsync(Compra compra, Guid empresaId);
    }
}
