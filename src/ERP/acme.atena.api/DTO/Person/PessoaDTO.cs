using acme.atena.domain.DTO.Account;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.DTO.Seguranca;
using System;
using System.Collections.Generic;
using System.Text;

namespace acme.atena.api.DTO.Person
{
    public abstract class PessoaDTO : AbstractEntity
    {
        public PessoaDTO()
        {
        }

        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }

        

    }
}
