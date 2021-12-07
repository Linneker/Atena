using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Interface.Service.Product
{
    public interface IDevolucaoCompraService: IServiceBase<DevolucaoCompra>
    {
        void Devolver(List<DevolucaoCompra> devolucaoCompra);
    }
}
