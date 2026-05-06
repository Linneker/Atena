using FluentValidation;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.CriarCentroDeCusto;

public sealed class CriarCentroDeCustoCommandValidation : AbstractValidator<CriarCentroDeCustoCommand>
{
    public CriarCentroDeCustoCommandValidation()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(30);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Descricao).MaximumLength(2000);
    }
}
