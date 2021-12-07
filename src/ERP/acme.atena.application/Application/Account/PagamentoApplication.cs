using acme.atena.application.Interface.Account;
using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Application.Account
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
