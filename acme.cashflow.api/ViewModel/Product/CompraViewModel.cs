using acme.cashflow.api.ViewModel.Person;
using acme.cashflow.api.ViewModel.Security;
using acme.cashflow.api.ViewModel.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.api.ViewModel.Product
{
    public class CompraViewModel : AbstractEntityViewModel
    {
        public CompraViewModel()
        {
            ComprasProdutos = new HashSet<CompraProdutoViewModel>();
        }

        public DateTime DataCompra { get; set; }
        public long ValorTotal { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid FornecedorId { get; set; }
        public Guid CompetenciaId { get; set; }

        public virtual UsuarioViewModel Usuario { get; set; }
        public virtual FornecedorViewModel Fornecedor { get; set; }
        public virtual CompetenciaViewModel Competencia { get; set; }
        public virtual ICollection<CompraProdutoViewModel> ComprasProdutos { get; set; }


    }
}
