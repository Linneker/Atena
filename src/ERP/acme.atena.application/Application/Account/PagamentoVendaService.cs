using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Service.Account
{
    public class PagamentoVendaService : ServiceBase<PagamentoVenda>, IPagamentoVendaService
    {
        public readonly IPagamentoVendaRepository _pagamentoVendaRepository;
        private readonly IPagamentoService _pagamentoService;

        public PagamentoVendaService(IPagamentoVendaRepository pagamentoVendaService, IPagamentoService _pagamentoService) : base(pagamentoVendaService)
        {
            _pagamentoVendaRepository = pagamentoVendaService;
        }

        public Task ConcluirPagamentoAsync(List<PagamentoVenda> pagamentoVendas, Venda venda)
        {
            Parallel.ForEach(pagamentoVendas, e =>
            {
                if (!e.Parcelado)
                {
                    _pagamentoVendaRepository.GetById(e.Id);
                    e.StatusPagamento = EnumStatusPagamento.Pago;

                    Pagamento pagamento = new Pagamento();
                    pagamento.Parcelado = e.Parcelado;
                    pagamento.CompetenciaId = e.CompetenciaId;
                    pagamento.ClienteId = venda.ClienteId;
                    pagamento.EmpresaId = venda.EmpresaId;
                    pagamento.UsuarioId = venda.UsuarioId;
                    pagamento.DataCredito = e.DataCredito;
                    pagamento.DataPagamento = e.DataPagamento;
                    pagamento.EnumTipoPagamento = EnumTipoPagamento.Realizado;
                    pagamento.NumeroDaParcela = 1;
                    pagamento.ValorPago = venda.ValorPago.HasValue ? venda.ValorPago.Value : venda.ValorTotal;
                    pagamento.PagamentoVendaId = e.Id;

                    foreach (var formPagamento in e.PagamentoVendasFormasPagamentos)
                    {
                        pagamento.PagamentosFormasPagamentos.Add(new PagamentoFormaPagamento(formPagamento.FormaPagamentoId));
                    }
                    _pagamentoService.AddAsync(pagamento);

                }
            });
            return Task.CompletedTask;
        }
    }
}
