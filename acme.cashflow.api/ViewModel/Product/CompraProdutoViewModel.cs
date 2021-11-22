using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.api.ViewModel.Product
{
    public class CompraProdutoViewModel : AbstractEntityViewModel
    {
        private decimal _valor;

        public decimal Valor { get => _valor; set => _valor = (value * QuantidadeComprada); }
        public int QuantidadeComprada { get; set; }

        public Guid ProdutoId { get; set; }
        public Guid CompraId { get; set; }

        public virtual ProdutoViewModel Produto { get; set; }
        public virtual CompraViewModel Compra { get; set; }

    }
}
