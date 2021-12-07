using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.domain.DTO.Person
{
    public class Fornecedor : Pessoa
    {
        public Fornecedor()
        {
            Compras = new HashSet<Compra>();
            EnderecoFornecedores = new HashSet<EnderecoFornecedor>();
        }
        
        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<Compra> Compras{ get; set; }
        public virtual ICollection<EnderecoFornecedor> EnderecoFornecedores{ get; set; }
    }
}
