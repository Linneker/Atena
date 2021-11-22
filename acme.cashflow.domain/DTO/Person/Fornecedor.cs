using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Person
{
    public class Fornecedor : AbstractEntity
    {
        public Fornecedor()
        {
            Compras = new HashSet<Compra>();
        }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<Compra> Compras{ get; set; }
        public virtual ICollection<Pagamento> Pagamentos { get; set; }
        public virtual ICollection<Divida> Dividas { get; set; }

    }
}
