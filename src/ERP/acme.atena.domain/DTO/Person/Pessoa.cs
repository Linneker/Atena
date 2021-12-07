using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Person
{
    public abstract class Pessoa : AbstractEntity
    {
        public Pessoa()
        {
            Dividas = new HashSet<Divida>();
            Pagamentos = new HashSet<Pagamento>();
        }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        public virtual ICollection<Pagamento> Pagamentos{ get; set; }
        public virtual ICollection<Divida> Dividas { get; set; }
        

    }
}
