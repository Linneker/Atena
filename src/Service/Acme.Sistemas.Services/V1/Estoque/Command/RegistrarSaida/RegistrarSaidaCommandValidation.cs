using FluentValidation;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;

public sealed class RegistrarSaidaCommandValidation : AbstractValidator<RegistrarSaidaCommand>
{
    public RegistrarSaidaCommandValidation()
    {
        RuleFor(x => x.EstoqueId).NotEmpty();
        RuleFor(x => x.ProdutoId).NotEmpty();
        RuleFor(x => x.Quantidade).GreaterThan(0);
        RuleFor(x => x.CustoUnitario).GreaterThanOrEqualTo(0).When(x => x.CustoUnitario.HasValue);
        RuleFor(x => x.Motivo).MaximumLength(500);
    }
}
