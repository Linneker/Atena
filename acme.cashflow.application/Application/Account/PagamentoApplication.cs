using acme.cashflow.application.Interface.Account;
using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.application.Application.Account
{
    public class PagamentoApplication: ApplicationBase<Pagamento>, IPagamentoApplication
    {
        private readonly IPagamentoService _pagamentoService;

        public PagamentoApplication(IPagamentoService pagamentoService):base(pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }
    }
}
