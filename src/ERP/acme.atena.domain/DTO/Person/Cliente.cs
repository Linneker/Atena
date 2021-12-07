using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.domain.DTO.Person
{
    public class Cliente : Pessoa
    {
        public Cliente()
        {
            Vendas = new HashSet<Venda>();
            EnderecoClientes = new HashSet<EnderecoCliente>();

        }

        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public virtual ICollection<Venda> Vendas { get; set; }
        public virtual ICollection<EnderecoCliente> EnderecoClientes { get; set; }

    }
}
