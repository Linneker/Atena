using acme.atena.core.Message;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Cliente.Commands.Input
{
    public class CadastroClienteCommand: Command
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeFantasia { get; set; }
        public string InscricaoMunicipal { get; set; }

        public override ValidationResult EhValido()
        {
            return base.EhValido();
        }

    }
}
