using FluentValidation;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.CriarSolicitacao;

public sealed class CriarSolicitacaoCommandValidation : AbstractValidator<CriarSolicitacaoCommand>
{
    public CriarSolicitacaoCommandValidation()
    {
        RuleFor(x => x.Justificativa).MaximumLength(2000);
        RuleFor(x => x.Itens).NotEmpty().WithMessage("A solicitação deve ter pelo menos um item.");
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.ProdutoId).NotEmpty();
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
            i.RuleFor(x => x.PrecoEstimado).GreaterThanOrEqualTo(0).When(x => x.PrecoEstimado.HasValue);
        });
    }
}
