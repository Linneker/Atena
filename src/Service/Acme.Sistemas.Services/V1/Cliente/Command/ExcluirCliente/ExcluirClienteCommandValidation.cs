using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cliente.Command.ExcluirCliente;

public sealed class ExcluirClienteCommandValidation : AbstractValidator<ExcluirClienteCommand>
{
    public ExcluirClienteCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
