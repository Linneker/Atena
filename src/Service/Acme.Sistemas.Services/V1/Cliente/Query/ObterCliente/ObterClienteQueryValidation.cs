using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ObterCliente;

public sealed class ObterClienteQueryValidation : AbstractValidator<ObterClienteQuery>
{
    public ObterClienteQueryValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
