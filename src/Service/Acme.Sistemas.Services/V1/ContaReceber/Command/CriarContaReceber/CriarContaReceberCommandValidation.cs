using FluentValidation;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.CriarContaReceber;

public sealed class CriarContaReceberCommandValidation : AbstractValidator<CriarContaReceberCommand>
{
    public CriarContaReceberCommandValidation()
    {
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(500);
        RuleFor(x => x.ValorOriginal).GreaterThan(0);
        RuleFor(x => x.DataVencimento).NotEmpty();
    }
}
