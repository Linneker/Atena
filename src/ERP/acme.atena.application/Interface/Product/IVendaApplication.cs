using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Interface.Product
{
    public interface IVendaApplication : IApplicationBase<Venda>
    {
        void Conclusao(Venda venda, Guid empresaId);
    }
}
