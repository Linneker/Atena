using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Product;
using acme.cashflow.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Person
{
    public class Pessoa : AbstractEntity
    {
        public Pessoa()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Pagamento> Pagamentos{ get; set; }
        public virtual ICollection<Venda> Vendas{ get; set; }

    }
}
