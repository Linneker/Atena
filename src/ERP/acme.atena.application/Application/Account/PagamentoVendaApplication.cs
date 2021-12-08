using acme.atena.application.Interface.Account;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Application.Account
{
    public  class PagamentoVendaApplication: ApplicationBase<PagamentoVenda>, IPagamentoVendaApplication
    {
        public readonly IPagamentoVendaService _pagamentoVendaService;

        public PagamentoVendaApplication(IPagamentoVendaService pagamentoVendaService) : base(pagamentoVendaService)
        {
            _pagamentoVendaService = pagamentoVendaService;
        }

        public void ConcluirPagamentoAsync(List<PagamentoVenda> pagamentoVendas, Venda venda)
        {
            _pagamentoVendaService.ConcluirPagamentoAsync(pagamentoVendas, venda);
        }
    }
}
