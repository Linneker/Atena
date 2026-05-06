using FluentValidation;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;

public sealed class RegistrarEntradaCommandValidation : AbstractValidator<RegistrarEntradaCommand>
{
    public RegistrarEntradaCommandValidation()
    {
        RuleFor(x => x.EstoqueId).NotEmpty();
        RuleFor(x => x.ProdutoId).NotEmpty();
        RuleFor(x => x.Quantidade).GreaterThan(0);
        RuleFor(x => x.CustoUnitario).GreaterThanOrEqualTo(0).When(x => x.CustoUnitario.HasValue);
        RuleFor(x => x.Motivo).MaximumLength(500);
        RuleFor(x => x.DocumentoReferencia).MaximumLength(100);
    }
}
