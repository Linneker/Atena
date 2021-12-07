using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.Interface.Service.Product
{
    public interface IVendaService : IServiceBase<Venda>
    {
        void Conclusao(Venda venda, Guid empresaId);
    }
}
