using FluentValidation;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.ExcluirCentroDeCusto;

public sealed class ExcluirCentroDeCustoCommandValidation : AbstractValidator<ExcluirCentroDeCustoCommand>
{
    public ExcluirCentroDeCustoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
