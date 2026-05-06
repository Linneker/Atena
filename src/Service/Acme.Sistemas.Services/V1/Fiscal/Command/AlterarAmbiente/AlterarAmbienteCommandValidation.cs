using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.AlterarAmbiente;

public sealed class AlterarAmbienteCommandValidation : AbstractValidator<AlterarAmbienteCommand>
{
    public AlterarAmbienteCommandValidation()
    {
        RuleFor(x => x.Ambiente).IsInEnum();
    }
}
