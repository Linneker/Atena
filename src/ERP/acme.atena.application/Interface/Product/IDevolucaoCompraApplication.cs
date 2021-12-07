using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Product
{
    public interface IDevolucaoCompraApplication: IApplicationBase<DevolucaoCompra>
    {
        void Devolver(List<DevolucaoCompra> devolucaoCompra);
    }
}
