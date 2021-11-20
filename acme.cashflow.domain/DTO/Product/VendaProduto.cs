using acme.cashflow.domain.DTO.Person;
using acme.cashflow.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Product
{
    public class VendaProduto : AbstractEntity
    {
        private decimal _valor;

        public decimal Valor { get => _valor; set => _valor = (value * QuantidadeVedida); }
        public int QuantidadeVedida { get; set; }
        public Guid VendaId { get; set; }
        public DateTime DataPagamento { get; set; }

        public Guid ProdutoId { get; set; }

        public virtual Venda Venda { get; set; }
        public virtual Produto Produto { get; set; }

    }
}
