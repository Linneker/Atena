using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cst.Query.ListarCsts;

public sealed class ListarCstsQueryValidation : AbstractValidator<ListarCstsQuery>
{
    private static readonly string[] Tipos = { "icms", "pis", "cofins", "ipi" };

    public ListarCstsQueryValidation()
    {
        RuleFor(x => x.Tipo)
            .NotEmpty()
            .Must(t => Tipos.Contains(t.ToLowerInvariant()))
            .WithMessage("Tipo deve ser um de: icms, pis, cofins, ipi.");
    }
}
