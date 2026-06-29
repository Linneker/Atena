using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ObterConfiguracaoRep;

public sealed class ObterConfiguracaoRepQueryValidation : AbstractValidator<ObterConfiguracaoRepQuery>
{
    public ObterConfiguracaoRepQueryValidation()
    {
        RuleFor(x => x.EmpresaId).NotEmpty();
    }
}
