using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.api.ViewModel.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.api.ViewModel.Product
{
    public class VendaProdutoViewModel : AbstractEntityViewModel
    {
        private decimal _valor;

        public decimal Valor { get => _valor; set => _valor = (value * QuantidadeVedida); }
        public int QuantidadeVedida { get; set; }
        public Guid VendaId { get; set; }
        public DateTime DataPagamento { get; set; }

        public Guid ProdutoId { get; set; }

        public virtual VendaViewModel Venda { get; set; }
        public virtual ProdutoViewModel Produto { get; set; }

    }
}
