using FluentValidation;

namespace Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;

public sealed class GerarRecorrenciasDespesaCommandValidation : AbstractValidator<GerarRecorrenciasDespesaCommand>
{
    public GerarRecorrenciasDespesaCommandValidation()
    {
        RuleFor(x => x.Meses).InclusiveBetween(1, 24);
    }
}
