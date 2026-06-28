using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Command.PagarSaldo;

public sealed class PagarSaldoCommandValidation : AbstractValidator<PagarSaldoCommand>
{
    public PagarSaldoCommandValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$");
        RuleFor(x => x.Minutos).GreaterThan(0);
    }
}
