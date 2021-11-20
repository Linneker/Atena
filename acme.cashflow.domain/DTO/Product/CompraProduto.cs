using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Product
{
    public class CompraProduto : AbstractEntity
    {
        private decimal _valor;

        public decimal Valor { get => _valor; set => _valor = (value * QuantidadeComprada); }
        public int QuantidadeComprada { get; set; }

        public Guid ProdutoId { get; set; }
        public Guid CompraId { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual Compra Compra { get; set; }

    }
}
