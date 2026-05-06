using FluentValidation;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirNFe;

public sealed class EmitirNFeCommandValidation : AbstractValidator<EmitirNFeCommand>
{
    public EmitirNFeCommandValidation()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.Itens).NotEmpty();
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.ProdutoId).NotEmpty();
            i.RuleFor(x => x.Descricao).NotEmpty().MaximumLength(500);
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
            i.RuleFor(x => x.PrecoUnitario).GreaterThan(0);
            i.RuleFor(x => x.Ncm).MaximumLength(10);
            i.RuleFor(x => x.Cfop).MaximumLength(10);
        });
    }
}
