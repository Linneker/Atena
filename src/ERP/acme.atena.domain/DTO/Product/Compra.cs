using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Product
{
    public class Compra : AbstractEntityCompetencia
    {
        public Compra()
        {
            ComprasProdutos = new HashSet<CompraProduto>();
        }

        public DateTime DataCompra { get; set; }
        public long ValorTotal { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid FornecedorId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual ICollection<CompraProduto> ComprasProdutos { get; set; }


    }
}
