using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Interface.Product
{
    public interface ICompraApplication : IApplicationBase<Compra>
    {
        void CadastroAsync(Compra compra, Guid empresaId);
    }
}
