using acme.atena.application.Handler.Comum.Commands.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Fornecedores.Commands.Response
{
    public class FornecedorQuery: OutputAbstractEntityCommand
    {
        [Display(Name = "Nome Fantasia")]
        public string NomeFantasia { get; set; }
        [Display(Name = "IM")] 
        public string InscricaoMunicipal { get; set; }
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        [Display(Name = "Cpf/Cnpj")]
        public string CpfCnpj { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        public string Celular { get; set; }
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }
    }
}
