using FluentValidation;

namespace Acme.Sistemas.Services.V1.Cfop.Query.ListarCfops;

public sealed class ListarCfopsQueryValidation : AbstractValidator<ListarCfopsQuery>
{
    private static readonly string[] Categorias = { "Entrada", "Saida" };

    public ListarCfopsQueryValidation()
    {
        RuleFor(x => x.Categoria)
            .Must(c => string.IsNullOrWhiteSpace(c) || Categorias.Contains(c))
            .WithMessage("Categoria deve ser 'Entrada' ou 'Saida'.");
    }
}
