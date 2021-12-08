using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Account;
using acme.atena.domain.Interface.Service.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.Service.Account
{
    public  class PagamentoVendaService: ServiceBase<PagamentoVenda>, IPagamentoVendaService
    {
        private readonly IPagamentoVendaRepository _pagamentoVendaRepository;
        private readonly IPagamentoService _pagamentoService;

        public PagamentoVendaService(IPagamentoVendaRepository pagamentoVendaRepository, IPagamentoService pagamentoService):base(pagamentoVendaRepository)
        {
            _pagamentoVendaRepository = pagamentoVendaRepository;
            _pagamentoService = pagamentoService;
        }

        public void ConcluirPagamentoAsync(List<PagamentoVenda> pagamentoVendas, Venda venda)
        {
            Parallel.ForEach(pagamentoVendas, e => {
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

                    foreach(var formPagamento in e.PagamentoVendasFormasPagamentos)
                    {
                        pagamento.PagamentosFormasPagamentos.Add(new PagamentoFormaPagamento(formPagamento.FormaPagamentoId));
                    }
                    _pagamentoService.AddAsync(pagamento);

                }
            });

        }
    }
}
