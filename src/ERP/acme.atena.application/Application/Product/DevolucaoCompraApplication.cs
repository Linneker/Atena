using acme.atena.application.Interface.Product;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Product
{
    public class DevolucaoCompraApplication: ApplicationBase<DevolucaoCompra>, IDevolucaoCompraApplication
    {
        private readonly IDevolucaoCompraService _devolucaoCompraService;

        public DevolucaoCompraApplication(IDevolucaoCompraService devolucaoCompraService):base(devolucaoCompraService) 
        {
            _devolucaoCompraService = devolucaoCompraService;
        }

        public void Devolver(List<DevolucaoCompra> devolucaoCompra)
        {
            _devolucaoCompraService.Devolver(devolucaoCompra);
        }
    }
}
