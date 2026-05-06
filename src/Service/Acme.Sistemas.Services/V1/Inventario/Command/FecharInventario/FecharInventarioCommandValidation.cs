using FluentValidation;

namespace Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

public sealed class FecharInventarioCommandValidation : AbstractValidator<FecharInventarioCommand>
{
    public FecharInventarioCommandValidation()
    {
        RuleFor(x => x.InventarioId).NotEmpty();
        RuleFor(x => x.Contagens).NotNull();
        RuleForEach(x => x.Contagens).ChildRules(c =>
        {
            c.RuleFor(x => x.ProdutoId).NotEmpty();
            c.RuleFor(x => x.SaldoContado).GreaterThanOrEqualTo(0);
        });
    }
}
