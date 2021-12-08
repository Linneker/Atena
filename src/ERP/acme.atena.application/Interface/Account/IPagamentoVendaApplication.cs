﻿using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Interface.Account
{
    public interface IPagamentoVendaApplication : IApplicationBase<PagamentoVenda>
    {
        void ConcluirPagamentoAsync(List<PagamentoVenda> pagamentoVendas, Venda venda);
    }
}
