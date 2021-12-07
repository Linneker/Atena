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
    public class DevolucaoVendaApplication: ApplicationBase<DevolucaoVenda>, IDevolucaoVendaApplication
    {
        private readonly IDevolucaoVendaService _devolucaoVendaService;

        public DevolucaoVendaApplication(IDevolucaoVendaService devolucaoVendaService):base(devolucaoVendaService)
        {
            _devolucaoVendaService = devolucaoVendaService;
        }

        public void Devolver(List<DevolucaoVenda> devolucaoVenda)
        {
            _devolucaoVendaService.Devolver(devolucaoVenda);
        }
    }
}
