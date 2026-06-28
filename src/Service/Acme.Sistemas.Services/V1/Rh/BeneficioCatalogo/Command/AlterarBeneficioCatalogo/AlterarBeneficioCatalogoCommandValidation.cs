using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.AlterarBeneficioCatalogo;

public sealed class AlterarBeneficioCatalogoCommandValidation : AbstractValidator<AlterarBeneficioCatalogoCommand>
{
    public AlterarBeneficioCatalogoCommandValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Codigo).MaximumLength(20);
        RuleFor(x => x.DescontoFuncionarioPct).InclusiveBetween(0, 100)
            .When(x => x.DescontoFuncionarioPct.HasValue);
        RuleFor(x => x.CustoEmpresaPadrao).GreaterThanOrEqualTo(0)
            .When(x => x.CustoEmpresaPadrao.HasValue);
        RuleFor(x => x.NaturezaRubricaEsocial).MaximumLength(20);
    }
}
