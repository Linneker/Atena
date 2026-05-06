using FluentValidation;

namespace Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;

public sealed class CriarOrcamentoCommandValidation : AbstractValidator<CriarOrcamentoCommand>
{
    public CriarOrcamentoCommandValidation()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.DataValidade).NotEmpty()
            .GreaterThan(DateTime.UtcNow.AddMinutes(-5))
            .WithMessage("Data de validade deve ser futura.");
        RuleFor(x => x.DescontoPercentual).InclusiveBetween(0, 100).When(x => x.DescontoPercentual.HasValue);
        RuleFor(x => x.Itens).NotEmpty();
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.ProdutoId).NotEmpty();
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
            i.RuleFor(x => x.PrecoUnitario).GreaterThan(0);
        });
    }
}
