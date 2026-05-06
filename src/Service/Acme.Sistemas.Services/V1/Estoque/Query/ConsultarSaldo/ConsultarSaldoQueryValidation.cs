using FluentValidation;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;

public sealed class ConsultarSaldoQueryValidation : AbstractValidator<ConsultarSaldoQuery>
{
    public ConsultarSaldoQueryValidation()
    {
        RuleFor(x => x.ProdutoId).NotEmpty();
    }
}
