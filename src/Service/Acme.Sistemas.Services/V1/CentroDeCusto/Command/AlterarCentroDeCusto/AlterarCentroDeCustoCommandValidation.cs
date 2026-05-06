using FluentValidation;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.AlterarCentroDeCusto;

public sealed class AlterarCentroDeCustoCommandValidation : AbstractValidator<AlterarCentroDeCustoCommand>
{
    public AlterarCentroDeCustoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
    }
}
