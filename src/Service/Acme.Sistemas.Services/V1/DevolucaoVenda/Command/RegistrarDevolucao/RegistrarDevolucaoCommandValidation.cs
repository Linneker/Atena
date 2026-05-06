using FluentValidation;

namespace Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;

public sealed class RegistrarDevolucaoCommandValidation : AbstractValidator<RegistrarDevolucaoCommand>
{
    public RegistrarDevolucaoCommandValidation()
    {
        RuleFor(x => x.FaturamentoId).NotEmpty();
        RuleFor(x => x.EstoqueDestinoId).NotEmpty();
        RuleFor(x => x.Motivo).MaximumLength(2000);
        RuleFor(x => x.Itens).NotEmpty();
        RuleForEach(x => x.Itens).ChildRules(i =>
        {
            i.RuleFor(x => x.FaturamentoItemId).NotEmpty();
            i.RuleFor(x => x.Quantidade).GreaterThan(0);
        });
    }
}
