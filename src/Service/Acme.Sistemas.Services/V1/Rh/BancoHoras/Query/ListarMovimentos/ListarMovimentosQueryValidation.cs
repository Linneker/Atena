using FluentValidation;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarMovimentos;

public sealed class ListarMovimentosQueryValidation : AbstractValidator<ListarMovimentosQuery>
{
    public ListarMovimentosQueryValidation()
    {
        RuleFor(x => x.FuncionarioId).NotEmpty();
        RuleFor(x => x.Competencia).NotEmpty().Matches(@"^\d{4}-\d{2}$");
    }
}
