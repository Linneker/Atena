using FluentValidation;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

public sealed class GerarBalancoQueryValidation : AbstractValidator<GerarBalancoQuery>
{
    public GerarBalancoQueryValidation()
    {
        RuleFor(x => x.DataReferencia).NotEmpty();
    }
}
