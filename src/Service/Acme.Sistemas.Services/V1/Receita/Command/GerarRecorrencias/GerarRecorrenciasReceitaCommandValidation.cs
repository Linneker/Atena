using FluentValidation;

namespace Acme.Sistemas.Services.V1.Receita.Command.GerarRecorrencias;

public sealed class GerarRecorrenciasReceitaCommandValidation : AbstractValidator<GerarRecorrenciasReceitaCommand>
{
    public GerarRecorrenciasReceitaCommandValidation()
    {
        RuleFor(x => x.Meses).InclusiveBetween(1, 24);
    }
}
