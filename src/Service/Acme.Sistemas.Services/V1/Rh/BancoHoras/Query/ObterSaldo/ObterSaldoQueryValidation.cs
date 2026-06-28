using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ObterSaldo;

public sealed class ObterSaldoQueryValidation : AbstractValidator<ObterSaldoQuery>
{
    public ObterSaldoQueryValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$");
    }
}
