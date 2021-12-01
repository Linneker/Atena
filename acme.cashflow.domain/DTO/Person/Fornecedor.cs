using acme.cashflow.domain.DTO.Account;
using acme.cashflow.domain.DTO.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.cashflow.domain.DTO.Person
{
    public class Fornecedor : Pessoa
    {
        public Fornecedor()
        {
            Compras = new HashSet<Compra>();
        }
        
        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<Compra> Compras{ get; set; }
        
    }
}
