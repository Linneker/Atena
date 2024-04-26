using FluentValidation.Results;
using MediatR;

namespace acme.atena.core.Message
{
    public abstract class Command : Message, IRequest<bool>
    {
        public DateTime? DataHora { get; private set; }
        public ValidationResult? ValidationResult { get; private set; }
        public string? Chave { get; set; }
        protected Command()
        {
            DataHora = DateTime.UtcNow;
        }
        public virtual ValidationResult EhValido()
        {
            throw new NotImplementedException();
        }
    }
}
