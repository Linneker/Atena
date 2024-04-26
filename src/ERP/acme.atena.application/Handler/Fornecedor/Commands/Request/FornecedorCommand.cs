using acme.atena.application.Handler.Enderecos.Commands.Request;
using acme.atena.core.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Fornecedores.Commands.Request
{
    public class FornecedorCommand: Command
    {
        [Display(Name ="Norme Fantasia")]
        public string NomeFantasia { get; set; }
        [Display(Name = "Inscrição Municipal")]
        public string InscricaoMunicipal { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Cpf/Cnpj", Description = "Insira um Cpf/Cnpj valido")]
        public string CpfCnpj { get; set; }
        [Display(Name = "E-Mail",Description ="Insira um e-mail valido")] 
        public string Email { get; set; }
        public string Celular { get; set; }
        [Display(Name = "Data de Nascimento", Description = "Insira a data vencimento")]
        public DateTime DataNascimento { get; set; }

        public List<EnderecoFornecedorCommand> EnderecoFornecedorCommands { get; set; }

        public override FluentValidation.Results.ValidationResult EhValido()
        {
            return base.EhValido();
        }
    }
}
