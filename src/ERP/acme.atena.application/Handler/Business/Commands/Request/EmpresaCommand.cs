using acme.atena.core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Business.Commands.Request
{
    public class EmpresaCommand : Command
    {
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public DateTime DataNascimento { get; set; }
        public bool IsFilial { get; set; }
        public Guid? EmpresaMatrizId { get; set; }

    }
}
