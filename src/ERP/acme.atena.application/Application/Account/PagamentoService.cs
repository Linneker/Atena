using acme.atena.domain.DTO.Account;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.application.Service.Account
{
    public class PagamentoService: ServiceBase<Pagamento>, IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoService;

        public PagamentoService(IPagamentoRepository pagamentoService):base(pagamentoService)
        {
            _pagamentoService = pagamentoService;
        }
    }
}
