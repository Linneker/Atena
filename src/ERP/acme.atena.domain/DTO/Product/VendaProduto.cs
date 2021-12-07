using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Enum;
using acme.atena.domain.DTO.Person;
using acme.atena.domain.DTO.Seguranca;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Product
{
    public class VendaProduto : AbstractEntityCompetencia
    {
        private decimal _valor;

        public decimal Valor { get => _valor; set => _valor = (value * QuantidadeVedida); }
        public int QuantidadeVedida { get; set; }
        public DateTime DataPagamento { get; set; }
        
        public bool Enviado { get; set; } = false;
        public Guid VendaId { get; set; }
        public Guid ProdutoId { get; set; }

        public virtual Venda Venda { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual ICollection<DevolucaoVenda> DevolucoesVenda { get; set; } = new HashSet<DevolucaoVenda>();
    }
}
