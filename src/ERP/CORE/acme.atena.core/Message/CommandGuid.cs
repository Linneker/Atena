using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.core.Message
{
    public abstract class CommandGuid : Message, IRequest<Guid>
    {
        public DateTime? DataHora { get; private set; }
        public ValidationResult? ValidationResult { get; private set; }
        public string? Chave { get; set; }
        protected CommandGuid()
        {
            DataHora = DateTime.UtcNow;
        }
        public virtual ValidationResult EhValido()
        {
            throw new NotImplementedException();
        }

    }
}
