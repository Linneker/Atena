using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Account
{
    public class PagamentoApplication: ServiceBase<Pagamento>, IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoService;

        public PagamentoApplication(IPagamentoRepository pagamentoService):base(pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }
    }
}
