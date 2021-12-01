using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.cashflow.domain.DTO.Person
{
    public class Cliente : Pessoa
    {
        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

    }
}
